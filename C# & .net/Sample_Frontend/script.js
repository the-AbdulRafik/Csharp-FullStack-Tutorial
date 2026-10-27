const API_URL = "http://localhost:5031/api/accounts";

const accountsList = document.getElementById("accountsList");
const responseOutput = document.getElementById("responseOutput");
const transactionAccount = document.getElementById("transactionAccount");
const searchInput = document.getElementById("searchInput");
const createAccountForm = document.getElementById("createAccountForm");
const transactionForm = document.getElementById("transactionForm");
const loadAccountsButton = document.getElementById("loadAccountsButton");

let accounts = [];

async function sendRequest(url, options = {}) {
  const response = await fetch(API_URL + url, {
    headers: {
      "Content-Type": "application/json"
    },
    ...options
  });

  const text = await response.text();
  let data = text;

  try {
    data = text ? JSON.parse(text) : null;
  } catch {
    // Some API errors are returned as plain text.
  }

  responseOutput.textContent = typeof data === "string"
    ? data
    : JSON.stringify(data, null, 2);

  if (!response.ok) {
    throw new Error(
      typeof data === "string"
        ? data
        : data?.title || data?.message || `Request failed with status ${response.status}`
    );
  }

  return data;
}

async function loadAccounts() {
  try {
    accounts = await sendRequest("");
    displayAccounts();
  } catch (error) {
    console.error(error);
  }
}

function displayAccounts() {
  const searchText = searchInput.value.toLowerCase();

  const filteredAccounts = accounts.filter(account =>
    account.accountHolderName.toLowerCase().includes(searchText) ||
    account.accountNumber.toLowerCase().includes(searchText) ||
    account.accountType.toLowerCase().includes(searchText)
  );

  accountsList.innerHTML = filteredAccounts.map(account => `
    <article class="account-card">
      <div>
        <div class="account-name">${account.accountHolderName}</div>
        <div class="account-meta">
          ${account.accountNumber} · ${account.accountType}
        </div>
      </div>
      <div class="account-balance">
        ₹${Number(account.balance).toFixed(2)}
      </div>
      <button class="account-edit" data-edit-id="${account.id}" type="button">
        Edit
      </button>
      <button class="account-delete" data-id="${account.id}" type="button">
        Delete
      </button>
    </article>
  `).join("");

  transactionAccount.innerHTML = `
    <option value="">Select an account</option>
    ${accounts.map(account => `
      <option value="${account.id}">
        ${account.accountNumber} - ${account.accountHolderName}
      </option>
    `).join("")}
  `;
}

createAccountForm.addEventListener("submit", async event => {
  event.preventDefault();

  const form = new FormData(createAccountForm);
  const account = {
    accountHolderName: form.get("accountHolderName"),
    accountNumber: form.get("accountNumber"),
    accountType: form.get("accountType"),
    balance: Number(form.get("balance"))
  };

  try {
    await sendRequest("", {
      method: "POST",
      body: JSON.stringify(account)
    });

    createAccountForm.reset();
    await loadAccounts();
  } catch (error) {
    console.error(error);
  }
});

transactionForm.addEventListener("submit", async event => {
  event.preventDefault();

  const form = new FormData(transactionForm);
  const accountId = form.get("accountId");
  const amount = Number(form.get("amount"));
  const transactionType = event.submitter.dataset.transaction;

  try {
    await sendRequest(`/${accountId}/${transactionType}`, {
      method: "POST",
      body: JSON.stringify({ amount })
    });

    transactionForm.reset();
    await loadAccounts();
  } catch (error) {
    console.error(error);
  }
});

accountsList.addEventListener("click", async event => {
  const editButton = event.target.dataset.editId;
  const accountId = event.target.dataset.id;

  if (editButton) {
    editAccount(editButton);
    return;
  }

  if (!accountId) {
    return;
  }

  try {
    await sendRequest(`/${accountId}`, {
      method: "DELETE"
    });

    await loadAccounts();
  } catch (error) {
    console.error(error);
  }
});

async function editAccount(accountId) {
  const account = accounts.find(item => item.id === Number(accountId));
  const accountHolderName = prompt("Account holder name:", account.accountHolderName);
  const accountType = prompt("Account type:", account.accountType);

  if (!accountHolderName || !accountType) {
    return;
  }

  try {
    await sendRequest(`/${accountId}`, {
      method: "PUT",
      body: JSON.stringify({
        accountHolderName,
        accountType
      })
    });

    await loadAccounts();
  } catch (error) {
    console.error(error);
  }
}

loadAccountsButton.addEventListener("click", loadAccounts);
searchInput.addEventListener("input", displayAccounts);
loadAccounts();
