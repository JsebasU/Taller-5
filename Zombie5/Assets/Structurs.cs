/// <summary>
/// strucuts del zombie y el villager
/// </summary>
public struct DatosZombie
{
    public string gusto;
    public string nombre;
    public int edad;
}
public struct DatosVillager
{
    public string nombre;
    public int edad;

    static public implicit operator DatosZombie(DatosVillager c)
    {
        DatosZombie z = new DatosZombie();
        z.gusto = "Cerebros";
        z.edad = c.edad;
        z.nombre = "Zombie " + c.nombre;
        return z;
    }
}
