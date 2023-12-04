namespace Tutoria.Services.SecurityAPI.Helpers
{
    public static class EmailBody
    {
        public static string EmailStringBody(string email, string emailToken)
        {
            return $@"
<html >
<head>
    <title>Reestablecer contraseña</title>
</head>
<body style=""margin: 10px; font-family: Arial, Helvetica, sans-serif; text-align: center;"">
    <div style=""align-items: center; justify-content: center; background: linear-gradient(to top, #c9c9ff 50%, #6e6ef6 90%) no-repeat; width: 400px; margin: 0 auto; border-radius: 10px; padding: 20px;"">
        
        <h1 style=""text-align: center; margin-bottom: 10px;"">Reestablecer contraseña</h1>
        <hr>
        <p style=""display: block;"">Hola</p>
        <p style=""display: block;"">Hemos recibido una solicitud para modificar la contraseña del Sistema de tutorias EPIIS-UNSAAC.</p>
        <p style=""display: block;"">Por favor, haga clic en el enlace que se encuentra a continuación  para reestablecer su contraseña.</p>
        <a href=""http://localhost:4200/auth/resetPassword?email={email}&code={emailToken}"" style=""display: inline-block; background-color: blue; color: white; padding: 10px 20px; text-decoration: none; border-radius: 25px; margin-top: 10px; display: block;"">Restablecer contraseña</a>
        <p style=""text-align: center;"">Sistema de Tutoría</p>
        <p style=""text-align: center;"">EPIIS - UNSAAC</p>
    </div>
</body>
</html>

            ";

        }
    }
}
