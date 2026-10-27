function calculateBMI(weight, height){
    weight = parseFloat(weight);
    height = parseFloat(height);
    
    if (isNaN(weight) || isNaN(height) || weight <= 0 || height <= 0) {
        const resultElem = document.getElementById("bmiResult");
        if (resultElem) {
            resultElem.innerHTML = "Please enter valid positive numbers for weight and height.";
        }
        return;
    }
    // height is expected in meters, weight in kilograms
    height = height / 100; // Convert height from cm to meters if needed
    let bmi = weight / (height * height);
    console.log("Calculated BMI: " + bmi);
    let category = getBMICategory(bmi);

    const resultElem = document.getElementById("bmiResult");
    if (resultElem) {
        resultElem.innerHTML = "Your BMI is: " + bmi.toFixed(2) + " (" + category + ")";
    }
}

function getBMICategory(bmi) {
    if (bmi < 18.5) {
        return "Underweight";
    } else if (bmi >= 18.5 && bmi < 24.9) {
        return "Normal weight";
    } else if (bmi >= 25 && bmi < 29.9) {
        return "Overweight";
    } else {
        return "Obesity";
    }
}

let data = {};
function fun(username, weight, height) {
    data[username] = {
        weight: weight,
        height: height
    };
    console.log(data);
}

document.addEventListener("DOMContentLoaded", function () {
    const billForm = document.getElementById("billForm");
    if (billForm) {
        billForm.addEventListener("submit", function(event) {
            event.preventDefault();
            let total = 0;
            let selectedItems = document.querySelectorAll('input[name^="item"]:checked');
            selectedItems.forEach(function(item) {
                total += parseFloat(item.value);
            });
            const billOutput = document.getElementById("billOutput");
            if (billOutput) {
                billOutput.innerHTML = "Total: $" + total.toFixed(2);
            }
        });
    }
});
