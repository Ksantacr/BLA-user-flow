CREATE TABLE users
(
    id            SERIAL PRIMARY KEY,
    firstname     VARCHAR(255)        NOT NULL,
    lastname      VARCHAR(255) NULL,
    email         VARCHAR(255) UNIQUE NOT NULL,
    password_hash TEXT                NOT NULL
);


CREATE TABLE posts
(
    id          SERIAL PRIMARY KEY,
    title       VARCHAR(255) NOT NULL,
    description TEXT NULL,
    created_by   INT4         NOT NULL,
    created_at   TIMESTAMP    NOT NULL
);