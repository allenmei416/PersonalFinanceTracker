document.addEventListener("DOMContentLoaded", () => {
    const editModal = document.getElementById('editTransactionModal');

    if (editModal) {
        editModal.addEventListener('show.bs.modal', (event) => {
            const button = event.relatedTarget;
            if (!button) return;

            editModal.querySelector('input[name="TransactionId"]').value = button.dataset.id;
            editModal.querySelector('select[name="CategoryId"]').value = button.dataset.category;
            editModal.querySelector('input[name="Amount"]').value = button.dataset.amount;
            editModal.querySelector('input[name="Date"]').value = button.dataset.date;
            editModal.querySelector('textarea[name="Note"]').value = button.dataset.note;
        });
    }

    // Optional: client-side filter
    const filterInput = document.getElementById('filterInput');
    if (filterInput) {
        filterInput.addEventListener('keyup', function () {
            const filter = this.value.toLowerCase();
            document.querySelectorAll('table tbody tr').forEach(row => {
                row.style.display = row.innerText.toLowerCase().includes(filter) ? '' : 'none';
            });
        });
    }
});
