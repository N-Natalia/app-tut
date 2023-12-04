namespace Tutoria.Services.SecurityAPI.Helpers
{
    public class EmailNotificationReservaBody
    {
        public static string EmailStringBody(string email, string emailToken)
        {
            return $@"
<html >
<head>
    <title>Tienes una nueva solicitud reserva</title>
</head>
<body style=""margin: 10px; font-family: Arial, Helvetica, sans-serif; text-align: center;"">
    <div style=""align-items: center; justify-content: center; background: linear-gradient(to top, #c9c9ff 50%, #6e6ef6 90%) no-repeat; width: 400px; margin: 0 auto; border-radius: 10px; padding: 20px;"">
        
        <h1 style=""text-align: center; margin-bottom: 10px;"">Solicitud de reserva</h1>
        <hr>
        <p style=""display: block;"">Hola</p>
        <p style=""display: block;"">Tienes una nueva solicitud de reserva de tutoria academica por parte de uno de tus tutorados.</p>
        <p style=""display: block;"">Por favor, ingrese al sistema de tutorias para obtener mayor detalle de la reserva.</p>
        <p style=""text-align: center;"">Sistema de Tutoría</p>
        <p style=""text-align: center;"">EPIIS - UNSAAC</p>
    </div>
</body>
</html>

            ";

        }
    }
}
