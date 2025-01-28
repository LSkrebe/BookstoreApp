let page = 1; // Track the current page
let isLoading = false; // Prevent duplicate API calls

function generateRandomSeed() {
    document.getElementById("seed").value = Math.floor(Math.random() * 1000);
}

function fetchBooks(page) {
    const seed = document.getElementById("seed").value || 0;
    const avgLikes = document.getElementById("avgLikes").value || 5;
    const avgReviews = document.getElementById("avgReviews").value || 5;
    const locale = document.getElementById("locale").value;

    return fetch(`/api/books?page=${page}&seed=${seed}&avgLikes=${avgLikes}&avgReviews=${avgReviews}&locale=${locale}`)
        .then(response => response.json())
        .catch(error => {
            console.error("Error fetching books:", error);
            throw error;
        });
}

function appendBooksToTable(books, clearExisting = false) {
    const tableBody = document.querySelector("#booksTable tbody");

    if (clearExisting) {
        tableBody.innerHTML = ''; // Clear existing rows
    }

    books.forEach(book => {
        const row = document.createElement("tr");
        row.classList.add("book-row"); // Add class for easy styling
        row.innerHTML = `
            <td>${book.index}</td>
            <td>${book.isbn}</td>
            <td>${book.title}</td>
            <td>${book.author}</td>
            <td>${book.publisher}</td>
        `;

        // Create a row for expanded book details (hidden by default)
        const expandedRow = document.createElement("tr");
        expandedRow.classList.add("expanded-row");
        expandedRow.style.display = "none";  // Initially hidden
        expandedRow.innerHTML = `
            <td colspan="5">
                <div class="book-details">
                    <div class="book-details-left">
                        <img src="${book.coverImageUrl}" alt="Cover Image" />
                        <p><strong>Likes:</strong> ${book.likes}</p>
                        <p><strong>Reviews:</strong> ${book.reviews}</p>
                        <p><strong>Pages:</strong> ${book.pages}</p>
                    </div>
                    <div class="book-details-right">
                        <h2>${book.title}</h2>
                        <h3>By ${book.author}</h3>
                        <p></p>
                        <h4>Comments:</h4>
                        ${book.comments.map(comment => `<p>"${comment}"</p>`).join('')}
                    </div>
                </div>
            </td>
        `;

        // Append both regular row and expanded row (book details + comments)
        tableBody.appendChild(row);
        tableBody.appendChild(expandedRow);

        // Add click event to toggle book details
        row.addEventListener("click", () => toggleBookDetails(expandedRow, row));
    });
}

// Updated toggleBookDetails function to toggle the expanded row
function toggleBookDetails(expandedRow, row) {
    const isVisible = expandedRow.style.display === "table-row";
    expandedRow.style.display = isVisible ? "none" : "table-row";

    // Optionally, add some styles to indicate the active row
    row.classList.toggle("active", !isVisible);
}

function loadBooks(clearExisting = false) {
    if (isLoading) return;

    isLoading = true;
    document.getElementById('loading').style.display = 'block';

    fetchBooks(page)
        .then(data => {
            appendBooksToTable(data, clearExisting);
            if (!clearExisting) {
                page++; // Increment page for infinite scroll
            } else {
                page = 2; // Reset to page 2 for new seeds
            }
        })
        .finally(() => {
            document.getElementById('loading').style.display = 'none';
            isLoading = false;
        });
}

// Handle infinite scrolling
window.addEventListener('scroll', () => {
    if (window.innerHeight + window.scrollY >= document.body.offsetHeight - 200) {
        loadBooks();
    }
});

// Handle "Generate Books" button click
document.getElementById("generateBooksBtn").addEventListener("click", () => {
    page = 1; // Reset to the first page
    loadBooks(true); // Clear existing books and fetch with new parameters
});

// Initial load
loadBooks();
