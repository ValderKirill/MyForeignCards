//получение всех слов
async function getAllWords() {
    const response = await fetch("/api/words", {
        method: "GET",
        headers: { "Accept": "application/json" }
    });

    if (response.ok === true) {
        const words = await response.json();
        const rows = document.querySelector("tbody");
        words.forEach(word => rows.append(row(word)));
    }
    else {
        const error = await response.json();
        console.log(error.message);
    }
}

//получение всех слов нужной категории
async function getWordsByCategory(categoryId) {
    const response = await fetch(`/api/words/category/${categoryId}`, {
        method: "GET",
        headers: { "Accept": "application/json" }
    });

    if (response.ok === true) {
        const words = await response.json();
        const rows = document.querySelector("tbody");
        words.forEach(word => rows.append(row(word)));
    }
    else {
        const error = await response.json();
        console.log(error.message);
    }
}

//получение слова
async function getWord(id) {
    const response = await fetch(`/api/words/${id}`, {
        method: "GET",
        headers: { "Accept": "application/json" }
    });

    if (response.ok === true) {
        const wordModel = await response.json();

        document.getElementById("wordId").value = wordModel.id;
        document.getElementById("text").value = wordModel.text;
        document.getElementById("translation").value = wordModel.translation;
        document.getElementById("category").value = wordModel.categoryId ?? "";
    }
    else {
        const error = await response.json();
        console.log(error.message);
    }
}

//Добавление слова
async function addWord(text, translation, categoryId) {
    const response = await fetch("/api/words", {
        method: "POST",
        headers: { "Accept": "application/json", "Content-Type": "application/json" },
        body: JSON.stringify({
            text: text,
            translation: translation,
            categoryId: categoryId
        })
    });

    if (response.ok === true) {
        resetAddForm();
    }
    else {
        const error = await response.json();
        console.log(error.message);
    }
}

//Изменение слова
async function editWord(id, text, translation, categoryId) {
    const response = await fetch(`/api/words/${id}`, {
        method: "PUT",
        headers: { "Accept": "application/json", "Content-Type": "application/json" },
        body: JSON.stringify({
            text: text,
            translation: translation,
            categoryId: categoryId
        })
    })

    if (response.ok === true) {
        resetAddForm();
    }
    else {
        const error = await response.json();
        console.log(error.message);
    }
}

//Удаление слова
async function deleteWord(id) {
    const response = await fetch(`/api/words/${id}`, {
        method: "DELETE",
        headers: { "Accept": "application/json" }
    });

    if (response.ok === true) {
        if (document.getElementById("wordId").value === id) {
            resetAddForm();
        }
    }
    else {
        const error = await response.json();
        console.log(error.message);
    }
}

// создание строки для таблицы
function row(word) {

    const tr = document.createElement("tr");
    tr.setAttribute("data-rowid", word.id);

    const nameTd = document.createElement("td");
    nameTd.append(word.text);
    tr.append(nameTd);

    const ageTd = document.createElement("td");
    ageTd.append(word.translation);
    tr.append(ageTd);

    const linksTd = document.createElement("td");

    const editLink = document.createElement("button");
    editLink.append("Изменить");
    editLink.addEventListener("click", async () => await getWord(word.id));
    linksTd.append(editLink);

    const removeLink = document.createElement("button");
    removeLink.append("Удалить");
    removeLink.addEventListener("click", async () => {
        const tableCategoryId = document.getElementById("tableCategory").value;
        await deleteWord(word.id);
        await refreshTable(tableCategoryId);
    });

    linksTd.append(removeLink);
    tr.appendChild(linksTd);

    return tr;
}

function resetAddForm() {
    document.getElementById("wordId").value =
        document.getElementById("text").value =
        document.getElementById("translation").value = "";
}

function resetTable() {
    document.querySelectorAll("tr[data-rowid]")
        .forEach(row => row.remove());
}

document.getElementById("resetBtn").addEventListener("click", () => resetAddForm());

document.getElementById("saveBtn").addEventListener("click", async () => {
    const id = document.getElementById("wordId").value;
    const text = document.getElementById("text").value;
    const translation = document.getElementById("translation").value;
    const newCategoryId = document.getElementById("category").value || null;
    const tableCategoryId = document.getElementById("tableCategory").value;

    if (id === "") {
        await addWord(text, translation, newCategoryId);
    }
    else {
        await editWord(id, text, translation, newCategoryId);
    }

    await refreshTable(tableCategoryId);
})

document.getElementById("tableCategory").addEventListener("change", async () => {
    const categoryId = document.getElementById("tableCategory").value;
    await refreshTable(categoryId);
})

async function refreshTable(categoryId) {
    resetTable();

    if (categoryId === "") {
        await getAllWords();
    }
    else {
        await getWordsByCategory(categoryId);
    }
}

getAllWords();