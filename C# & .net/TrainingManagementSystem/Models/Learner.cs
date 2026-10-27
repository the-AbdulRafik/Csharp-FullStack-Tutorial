/* 
Create a Learner class with LearnerId (string), Name (string), ContactNumber (string), and Email (string).


*/ 


namespace LearnerModel;

public class Learner
{
    public string LearnerId { get; set;}
    public string Name { get; set; }
    public string ContactNumber { get; set; }
    public string Email { get; set; }

    public Learner(string learnerId, string name, string contactNumber, string email)
    {
        LearnerId = learnerId;
        Name = name;
        ContactNumber = contactNumber;
        Email = email;
    }
}