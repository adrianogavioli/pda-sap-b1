namespace Frattina.CrossCutting.Configuration
{
    public class AppSettings
    {
        public static AppSettings Current;

        public AppSettings()
        {
            Current = this;
        }

        public string SendGridUser { get; set; }
        public string SendGridKey { get; set; }
        public string SapBaseAddress { get; set; }
        public string SapRouteId { get; set; }
        public string SapSerieCliente { get; set; }
        public string SapGrupoClienteContribuinte { get; set; }
        public string SapGrupoClienteNaoContribuinte { get; set; }
        public string SapGrupoFotoPrincipal { get; set; }
        public string SapImageDefaultPath { get; set; }
        public string SapImageThumbnailPath { get; set; }
        public string SapCompanyDB { get; set; }
        public string SapUserName { get; set; }
        public string SapPassword { get; set; }
        public string SapLanguage { get; set; }
        public string KissLogOrganizationId { get; set; }
        public string KissLogApplicationId { get; set; }
    }
}
