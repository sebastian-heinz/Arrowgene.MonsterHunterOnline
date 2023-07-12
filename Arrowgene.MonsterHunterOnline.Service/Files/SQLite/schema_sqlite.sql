CREATE TABLE IF NOT EXISTS `setting`
(
    `key`   TEXT NOT NULL,
    `value` TEXT NOT NULL,
    PRIMARY KEY (`key`)
);

CREATE TABLE IF NOT EXISTS `account`
(
    `id`            INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
    `uin`           INTEGER                           NOT NULL,
    `password_hash` TEXT                              NOT NULL,
    `created`       DATETIME                          NOT NULL,
    CONSTRAINT `uq_account_uin` UNIQUE (`uin`)
);

CREATE TABLE IF NOT EXISTS `character`
(
    `id`         INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
    `account_id` INTEGER                           NOT NULL,
    `name`       TEXT                              NOT NULL,
    `created`    DATETIME                          NOT NULL,
    CONSTRAINT `fk_character_account_id` FOREIGN KEY (`account_id`) REFERENCES `account` (`id`) ON DELETE CASCADE,
    CONSTRAINT `uq_character_name` UNIQUE (`name`)
);
