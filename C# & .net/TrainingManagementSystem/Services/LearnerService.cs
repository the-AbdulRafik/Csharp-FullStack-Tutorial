using LearnerModel;

namespace LearnerServicePkg;

public class LearnerService
{
    private Dictionary<string, Learner> learners = new Dictionary<string, Learner>(StringComparer.OrdinalIgnoreCase);

    public LearnerService(IEnumerable<Learner>? initialLearners = null)
    {
        if (initialLearners == null)
        {
            return;
        }

        foreach (Learner learner in initialLearners)
        {
            Register(learner);
        }
    }

    public Dictionary<string, Learner> Learners => learners;

    public void Register(Learner learner)
    {
        if (learner == null || string.IsNullOrWhiteSpace(learner.LearnerId) || string.IsNullOrWhiteSpace(learner.Name))
        {
            throw new ArgumentException("Learner ID and name are required.");
        }

        if (!learners.TryAdd(learner.LearnerId, learner))
        {
            throw new InvalidOperationException($"Learner {learner.LearnerId} already exists.");
        }
    }

    public bool TryGet(string? learnerId, out Learner? learner)
    {
        learner = null;
        return !string.IsNullOrWhiteSpace(learnerId) && learners.TryGetValue(learnerId.Trim(), out learner);
    }

    public void DisplayLearners()
    {
        foreach (Learner learner in learners.Values)
        {
            Console.WriteLine($"{learner.LearnerId}: {learner.Name} | {learner.Email}");
        }
    }
}
