namespace Web.Framework
{
    public class IdGenerator
    {
        public IdGenerator()
        {
            var machineId = Convert.ToUInt16(Globals.Configuration["WorkerID"] ?? "1");
            var options = new IdGeneratorOptions(machineId);
            YitIdHelper.SetIdGenerator(options);
        }

        public static string NewId => YitIdHelper.NextId().ToString();
    }
}
