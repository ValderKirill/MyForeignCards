//Добавление категории
async function addCategory(name) {
    const response = await fetch("/api/categories", {
        method: "POST",
        headers: { "Accept": "application/json", "Content-Type": "application/json" },
        body: JSON.stringify({
            name: name
        })
    });

    if (response.ok === true) {
        const category = await response.json();
        const selectCategory = document.getElementById("category");
        const selectTableCategory = document.getElementById("tableCategory");

        selectCategory.appendChild(createCategoryOption(category));
        selectTableCategory.appendChild(createCategoryOption(category));
        resetAddCategory();
    }
    else {
        const error = await response.json();
        console.log(error.message);
    }
}

//получение всех категорий
async function getAllCategories() {
    const response = await fetch("/api/categories", {
        method: "GET",
        headers: { "Accept": "application/json" }
    });

    if (response.ok === true) {
        const categories = await response.json();
        const selectCategory = document.getElementById("category");
        const selectTableCategory = document.getElementById("tableCategory");

        for (const category of categories) {
            selectCategory.appendChild(createCategoryOption(category));
            selectTableCategory.appendChild(createCategoryOption(category));
        }
    }
    else {
        const error = await response.json();
        console.log(error.message);
    }
}

async function deleteCategory(id) {
    const response = await fetch(`/api/categories/${id}`, {
        method: "DELETE",
        headers: { "Accept": "application/json" }
    });

    if (response.ok === true) {
        const options = document.querySelectorAll(`option[value='${id}']`);
        for (const option of options) {
            option.remove()
        }
        resetCategory();
    }
    else {
        const error = await response.json();
        console.log(error.message);
    }
}

function createCategoryOption(category) {
    const option = document.createElement("option");
    option.value = category.id;
    option.textContent = category.name;
    return option;
}

function resetAddCategory() {
    document.getElementById("newCategory").value = "";
}

function resetCategory() {
    document.getElementById("category").value = "";
}

function disableOrEnableDeleteCategoryButton() {
    const categoryId = document.getElementById("category").value;
    if (categoryId === "") {
        document.getElementById("deleteCategory").disabled = true;
    }
    else {
        document.getElementById("deleteCategory").disabled = false;
    }
}

document.getElementById("addCategoryBtn").addEventListener("click", async () => {
    const name = document.getElementById("newCategory").value;
    await addCategory(name);
})

document.getElementById("deleteCategory").addEventListener("click", async () => {
    const id = document.getElementById("category").value;
    await deleteCategory(id);
})

document.getElementById("category").addEventListener("change", async () => {
    disableOrEnableDeleteCategoryButton();
})

disableOrEnableDeleteCategoryButton();
getAllCategories();