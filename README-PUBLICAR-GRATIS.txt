SERVICIO DE CONEXIÓN — PUBLICACIÓN GRATUITA EN RENDER

Estado: código compilado y probado localmente. NO está alojado ni aprobado por Meta/Google. El instalador 0.10.0 todavía muestra el servicio como pendiente porque src/connection-service.json tiene url vacía.

ESTO SE HACE UNA VEZ POR EL RESPONSABLE DEL PROGRAMA, NO EN CADA PC:
1. Crear una cuenta en https://render.com/ . Elegir siempre el plan Free; no contratar un plan de pago ni activar cobros. Revisar las condiciones vigentes de la cuenta.
2. Crear un repositorio propio que contenga sólo esta carpeta connection-service. No subir archivos .env, credenciales, sesiones de OBS ni el contenido de AppData.
3. En Render crear un Web Service desde ese repositorio: Runtime Docker, plan Free. El Dockerfile incluido construye el servicio. También se incluye render.yaml para desplegar mediante Blueprint.
4. Configurar PUBLIC_URL con la dirección HTTPS que asigna Render (sólo origen, sin ruta). No se necesita comprar un dominio para hacer la prueba. Revisar requisitos de dominios/verificación que luego exijan las plataformas para producción.
5. Variables secretas, configuradas únicamente en Render: META_APP_ID, META_APP_SECRET, GOOGLE_CLIENT_ID y GOOGLE_CLIENT_SECRET. META_API_VERSION=v26.0. No ponerlas en el repositorio ni en el EXE. No pegar claves en el chat.
6. En Meta registrar exactamente https://TU-SERVICIO.onrender.com/callback/facebook. Conservar los retornos locales existentes si todavía los necesitás para desarrollo. Completar revisión de permisos y acceso para otros usuarios; publicar el servicio no elimina los requisitos de Meta.
7. En Google crear un cliente OAuth de tipo Aplicación web PARA EL SERVICIO; el cliente de escritorio anterior no sirve para un retorno HTTPS remoto. Registrar https://TU-SERVICIO.onrender.com/callback/google. Completar consentimiento, usuarios de prueba y verificación/publicación según los permisos de YouTube y usuarios objetivo. No hace falta cambiar la aplicación de escritorio anterior durante las pruebas.
8. Comprobar https://TU-SERVICIO.onrender.com/capabilities : protocol=1, facebook=true, google=true. Esto sólo confirma configuración, no aprobación ni permisos reales.
9. En el código del programa ejecutar CONFIGURAR-CONEXION-COMUN.ps1 -Url https://TU-SERVICIO.onrender.com y luego build-complete.ps1. Esto incrusta sólo la dirección pública en el nuevo EXE. Ningún secreto de aplicación viaja al instalador.
10. Probar en una instalación nueva: Conectar Facebook y Conectar YouTube. El usuario autoriza en el navegador y vuelve al panel. Probar rechazo, expiración, renovación y una transmisión privada antes de entregar a terceros.

EXPERIENCIA DEL USUARIO CUANDO EL SERVICIO ESTÉ PUBLICADO Y AUTORIZADO:
Instalar → elegir perfil/página si corresponde → Conectar Facebook → autorizar → Conectar YouTube → autorizar. La señal de YouTube se selecciona o crea automáticamente sin emitir. Si tiene varias páginas, elige una. Para configurar el archivo de Aitum necesita cerrar OBS una vez, con el panel abierto en el navegador, pulsar Preparar Aitum automáticamente y reabrir OBS. Debe decidir audiencia y si su contenido está creado para niños. No se acepta ningún permiso en su nombre.
El modo asistido usa el navegador normal y el retorno del servidor HTTPS: no requiere un certificado local de Facebook en cada PC ni registrar una dirección distinta por cada instalación.

LÍMITES DEL PLAN GRATUITO:
Render suspende el servicio tras 15 minutos sin actividad; al volver a conectar puede tardar en despertar. Las horas y recursos gratuitos tienen cuotas. Si el servicio se reinicia durante una autorización hay que repetirla. No guardar secretos o tokens en el sistema de archivos efímero. No se hace tráfico artificial para evitar la suspensión. Revisar https://render.com/docs/free . No se garantiza gratuidad ilimitada ni disponibilidad permanente; no se contrató ningún servicio.
No pasa video por el servicio. OBS envía directamente a Facebook/YouTube. La renovación de autorización de Google sí necesita el servicio disponible.

SEGURIDAD Y PRIVACIDAD:
Los secretos de aplicación permanecen en las variables del alojamiento. Las autorizaciones se retienen en memoria hasta un máximo de 10 minutos mientras están pendientes y 3 minutos después de completar el permiso, se canjean una vez con prueba SHA256 de la PC y se eliminan del registro activo. La limpieza es oportunista con la siguiente solicitud; no se persisten en disco ni se imprimen en logs de la aplicación. El proveedor de alojamiento puede tener sus propios registros; revisar sus condiciones y retención.
En la PC se guardan cifradas con DPAPI para ese usuario de Windows. La renovación de Google envía temporalmente el refresh token por HTTPS al mismo servicio configurado. Cambiar de servicio exige volver a autorizar. No abrir autorización contra un servicio que no controles.
Antes de publicar para terceros, el responsable debe añadir sus datos de contacto, política de privacidad y procedimiento de eliminación/revocación de accesos; no hay una política legal inventada o aprobada automáticamente. Los usuarios pueden revocar desde la configuración de Facebook/Google. La app no borra los videos publicados al desvincular o revocar.

PRUEBAS:
SessionStore y cliente: prueba de posesión, expiración, canje/callback de un solo uso, rechazos y ausencia de secretos en estado público. Servicio HTTP probado localmente sin contactar a Meta/Google. No se verificó una cuenta real con el servicio alojado porque todavía no existe alojamiento configurado.
