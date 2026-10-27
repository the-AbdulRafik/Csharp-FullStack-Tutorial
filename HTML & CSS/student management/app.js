let Students = [];

function addStudent(event) {
    event.preventDefault();

    const nameInput = document.getElementById("name");
    const ageInput = document.getElementById("age");
    const gradeInput = document.getElementById("grade");

    if (!nameInput || !ageInput || !gradeInput) {
        return;
    }

    const student = {
        name: nameInput.value.trim(),
        age: parseInt(ageInput.value, 10),
        grade: gradeInput.value.trim()
    };

    Students.push(student);
    document.getElementById("addStudentForm").reset();
    alert("Student added successfully!");
    renderStudents();
}

function renderStudents() {
    const list = document.getElementById("studentList");
    if (!list) {
        return;
    }
    list.innerHTML = "";

    for (let i = 0; i < Students.length; i++) {
        const row = document.createElement("tr");

        const nameCell = document.createElement("td");
        nameCell.textContent = Students[i].name;

        const ageCell = document.createElement("td");
        ageCell.textContent = Students[i].age;

        const gradeCell = document.createElement("td");
        gradeCell.textContent = Students[i].grade;

        row.appendChild(nameCell);
        row.appendChild(ageCell);
        row.appendChild(gradeCell);

        list.appendChild(row);
    }
}

function getStudents() {
    if (Students.length === 0) {
        alert("No students found.");
        return;
    }
    renderStudents();
}

function deleteStudent(name) {
    if (!name) {
        alert("Name is required to delete a student.");
        return;
    }
    const initialCount = Students.length;
    Students = Students.filter(student => student.name.toLowerCase() !== name.toLowerCase());

    if (Students.length < initialCount) {
        alert("Student with name " + name + " has been deleted.");
        renderStudents();
    } else {
        alert("Student with name " + name + " was not found.");
    }
}

document.addEventListener("DOMContentLoaded", function () {
    const deleteForm = document.getElementById("deleteStudentForm");
    if (deleteForm) {
        deleteForm.addEventListener("submit", function (e) {
            e.preventDefault();
            const deleteInput = document.getElementById("deleteName");
            if (deleteInput) {
                deleteStudent(deleteInput.value.trim());
                deleteForm.reset();
            }
        });
    }
});
