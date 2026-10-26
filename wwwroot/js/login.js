async function entry(login, password) {
    const response = await fetch("/login", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({
            login: login,
            password: password
        })
    });

    if (response.ok === true) {
        sessionStorage.setItem("username", login);
        window.location.href = "/index.html";
    }
    else {
        console.log("Status: ", response.status);
    }
}

async function registration(login, password) {
    const response = await fetch("/registration", {
        method: "POST",
        headers: { "Accept": "application/json", "Content-Type": "application/json" },
        body: JSON.stringify({
            login: login,
            password: password
        })
    });

    if (response.ok === true) {
        entry(login, password);
        sessionStorage.setItem("username", login);
        window.location.href = "/index.html";
    }
    else {
        console.log("Status: ", response.status);
    }
}

async function logout() {
    const response = await fetch("/logout", {
        method: "POST"
    });

    if (response.ok === true) {
        window.location.href = "login.html";
    }
    else {
        console.log("Status: ", response.status);
    }
}

function showUserInfo() {
    const login = sessionStorage.getItem("username");

    if (login) {
        document.getElementById("userName").innerText = login;
        document.getElementById("userInfo").style.display = "block";
    }
}

const loginBtn = document.getElementById("loginBtn");

if (loginBtn) {
    loginBtn.addEventListener("click", async e => {
        const login = document.getElementById("login").value;
        const password = document.getElementById("password").value;

        entry(login, password);
    });

    document.getElementById("regBtn").addEventListener("click", async e => {
        const login = document.getElementById("login").value;
        const password = document.getElementById("password").value;

        registration(login, password);
    });
}

const logoutBtn = document.getElementById("logOutBtn");

if (logoutBtn) {
    logoutBtn.addEventListener("click", async e => {
        logout();
    });

    showUserInfo();
}