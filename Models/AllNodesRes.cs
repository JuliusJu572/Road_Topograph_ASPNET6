using System.ComponentModel.DataAnnotations;

public class AllNodesRes
{
    public int id { get; set; }
    public string node_id { get; set; }

    public string hub_id { get; set; }

    public double span { get; set; }
    public double span2hub { get; set; }
    public string direction { get; set; }
    public string type { get; set; }
}
