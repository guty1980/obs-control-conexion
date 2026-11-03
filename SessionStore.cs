using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Nodes;
sealed class SessionStore {
 internal sealed class Session {public required string Provider;public required string Kind;public required string Challenge;public required string OAuthVerifier;public DateTimeOffset Until=DateTimeOffset.UtcNow.AddMinutes(10);public bool Processing;public JsonObject? Grant;public string Error="";}
 readonly Dictionary<string,Session> pending=[];readonly object sync=new();
 public static string Nonce()=>Convert.ToBase64String(RandomNumberGenerator.GetBytes(32)).TrimEnd('=').Replace('+','-').Replace('/','_');
 public static string Challenge(string verifier)=>Convert.ToBase64String(SHA256.HashData(Encoding.ASCII.GetBytes(verifier))).TrimEnd('=').Replace('+','-').Replace('/','_');
 public (string,Session) Create(string provider,string kind,string challenge){if(provider is not ("facebook" or "google")||kind is not ("page" or "profile")||!System.Text.RegularExpressions.Regex.IsMatch(challenge,@"^[A-Za-z0-9_-]{43}$"))throw new ArgumentException("Solicitud inválida");lock(sync){Prune();if(pending.Count>=256)throw new InvalidOperationException("Servicio ocupado; intentá más tarde");var id=Nonce();var session=new Session{Provider=provider,Kind=kind,Challenge=challenge,OAuthVerifier=Nonce()};pending[id]=session;return(id,session);}}
 void Prune(){foreach(var key in pending.Where(p=>p.Value.Until<DateTimeOffset.UtcNow).Select(p=>p.Key).ToArray())pending.Remove(key);}
 public Session Claim(string id,string provider){lock(sync){Prune();if(!pending.TryGetValue(id,out var s)||s.Provider!=provider||s.Processing)throw new InvalidOperationException("La autorización venció o ya fue utilizada");s.Processing=true;return s;}}
 public void Finish(string id,JsonObject? grant,string error=""){lock(sync){if(!pending.TryGetValue(id,out var s))return;s.Grant=grant;s.Error=error;s.Until=DateTimeOffset.UtcNow.AddMinutes(3);}}
 public (string Status,JsonObject? Grant) Redeem(string id,string verifier){if(!System.Text.RegularExpressions.Regex.IsMatch(verifier,@"^[A-Za-z0-9_-]{43}$"))throw new ArgumentException("Verificación inválida");lock(sync){Prune();if(!pending.TryGetValue(id,out var s))throw new InvalidOperationException("La autorización venció o ya fue recibida");if(!CryptographicOperations.FixedTimeEquals(Encoding.ASCII.GetBytes(Challenge(verifier)),Encoding.ASCII.GetBytes(s.Challenge)))throw new InvalidOperationException("Verificación rechazada");if(s.Error.Length>0){pending.Remove(id);throw new InvalidOperationException(s.Error);}if(s.Grant==null)return("waiting",null);pending.Remove(id);return("ready",s.Grant);}}
}
