CREATE DATABASE IF NOT EXISTS gamemode;
USE gamemode;

CREATE TABLE IF NOT EXISTS players (
  id int(11) NOT NULL AUTO_INCREMENT,
  name varchar(20) NOT NULL
  CHECK(
    TRIM(name) != '' AND 
    LENGTH(name) >= 3 AND 
    LENGTH(name) <= 20 AND 
    name REGEXP '^[0-9a-zA-Z\\[\\]\\(\\)\\$\\@\\.=_]+$'
  ),
  password varchar(60) NOT NULL CHECK(TRIM(password) != ''),
  total_kills int(11) NOT NULL CHECK(total_kills >= 0),
  total_deaths int(11) NOT NULL CHECK(total_deaths >= 0),
  max_killing_spree int(11) NOT NULL CHECK(max_killing_spree >= 0),
  brought_flags int(11) NOT NULL CHECK(brought_flags >= 0),
  captured_flags int(11) NOT NULL CHECK(captured_flags >= 0),
  dropped_flags int(11) NOT NULL CHECK(dropped_flags >= 0),
  returned_flags int(11) NOT NULL CHECK(returned_flags >= 0),
  head_shots int(11) NOT NULL CHECK(head_shots >= 0),
  role_id enum('Basic','VIP','Moderator','Admin') NOT NULL,
  skin_id int(11) NOT NULL CHECK(skin_id >= -1 AND skin_id <= 311),
  rank_id int(11) NOT NULL CHECK(rank_id >= 0 AND rank_id <= 14),
  created_at datetime NOT NULL,
  last_connection datetime NOT NULL,
  PRIMARY KEY (id),
  UNIQUE KEY ix_players_name (name) USING BTREE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;