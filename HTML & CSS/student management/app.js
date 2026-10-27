let Students = [];

let student = {
    name,
    age,
    grade,
}


function addStudent(event) {
    event.preventDefault();

    let student = {
        name,
        age,
        grade,
    }
    Students.push(student);
    alert("Student added successfully!");
}

function getStudents() {
    if (Students.length === 0) {
        alert("No students found.");
        return;
    }

    for (let i = 0; i < Students.length; i++) {
        document.getElementById("studentList").appendChild(document.createAttribute("td")).innerHTML = Students[i].name + " - Age: " + Students[i].age + ", Grade: " + Students[i].grade;
    
}

function deleteStudent(name) {
    if (!name) {
        alert("Name is required to delete a student.");
        return;
    }
    Students = Students.filter(student => student.name !== name);

    alert("Student with name " + name + " has been deleted.");
}




