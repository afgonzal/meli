namespace ML.FichaTecnica.BusinessEntities
{
    public class ComponentOutput
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }

        public ComponentTypes ComponentType { get; set; }
    }
}