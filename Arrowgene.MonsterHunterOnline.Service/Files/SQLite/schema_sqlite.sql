-- TINYINT  = 1byte
-- SMALLINT = 2 byte

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
    `id`                       INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
    `account_id`               INTEGER                           NOT NULL,
    `role_index`               TINYINT                           NOT NULL,
    `gender`                   TINYINT                           NOT NULL,
    `level`                    INTEGER                           NOT NULL,
    `name`                     TEXT                              NOT NULL,
    `role_state`               INTEGER                           NOT NULL,
    `role_state_end_left_time` INTEGER                           NOT NULL,
    `avatar_set_id`            TINYINT                           NOT NULL,
    `face_id`                  SMALLINT                          NOT NULL,
    `hair_id`                  SMALLINT                          NOT NULL,
    `underclothes_id`          SMALLINT                          NOT NULL,
    `skin_color`               INTEGER                           NOT NULL,
    `hair_color`               INTEGER                           NOT NULL,
    `inner_color`              INTEGER                           NOT NULL,
    `eye_ball`                 INTEGER                           NOT NULL,
    `eye_color`                INTEGER                           NOT NULL,
    `face_tattoo_index`        INTEGER                           NOT NULL,
    `face_tattoo_color`        INTEGER                           NOT NULL,
    `facial_info`              BLOB                              NOT NULL,
    `star_level`               TEXT                              NOT NULL,
    `hr_level`                 INTEGER                           NOT NULL,
    `soul_stone_lv`            INTEGER                           NOT NULL,
    `hide_helm`                TINYINT                           NOT NULL,
    `hide_fashion`             TINYINT                           NOT NULL,
    `hide_suite`               TINYINT                           NOT NULL,
    `created`                  DATETIME                          NOT NULL,
    CONSTRAINT `fk_character_account_id` FOREIGN KEY (`account_id`) REFERENCES `account` (`id`) ON DELETE CASCADE,
    CONSTRAINT `uq_character_name` UNIQUE (`name`),
    CONSTRAINT `uq_character_account_id_role_index` UNIQUE (`account_id`, `role_index`)
);
