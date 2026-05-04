using GearrOnes.HOA.Template.Config.Models;

namespace GearrOnes.HOA.Template.Config.Interfaces;

public interface IPlanConfigurationLoader
{
    PlanConfiguration Load(string path);
}