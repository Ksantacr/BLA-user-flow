INSERT INTO users (firstname, lastname, email, password_hash)
VALUES ('kevin', 'santacruz', 'kevin@admin.com', '$2a$11$qVKKPwIIS9h3q10SvdXoWOkXlTK4MbOVuvtObyLGIgga9IhtRo9Sq');


INSERT INTO posts (title, description, created_by, created_at)
VALUES ('My post 1', 'lorem ipsum', 1, CURRENT_TIMESTAMP),
       ('My post 2', NULL, 1, CURRENT_TIMESTAMP);