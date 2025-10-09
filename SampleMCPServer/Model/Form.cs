namespace SampleMCPServer.Model
{
    public class Form
    {
        public virtual Guid Id { get; set; }
        public virtual string Context { get; set; }
        public virtual int NumberOfRows { get; set; }   
        public virtual RowStructure[] FormStructure { get; set; }
    }

    public class RowStructure
    {
        public virtual int NumberOfFields { get; set; }
        public virtual FieldStructure[] Fields{ get; set; }
    }

    public class FieldStructure
    {
        public virtual string Label { get; set; }
        public virtual string InputType { get; set; }
        public virtual string[] Options { get; set; }
    }
}
