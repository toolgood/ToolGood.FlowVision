package toolgood.flowVision.Flows;

public class FactoryWork {
    private ProjectWork Project;// { get; set; }
    public String Category;
    public String Code;
    public String Name;
    public String SimplifyName;
    public String LongAndLat;

    public void Init(ProjectWork work)
    {
        Project = work;
    }
}
