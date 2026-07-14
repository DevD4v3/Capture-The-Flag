BEGIN TRANSACTION;
CREATE TABLE IF NOT EXISTS players (
  id INTEGER,
  name TEXT NOT NULL UNIQUE COLLATE NOCASE 
  CHECK(
    TRIM(name) != '' AND 
    LENGTH(name) >= 3 AND 
    LENGTH(name) <= 20 AND
    name REGEXP '^[0-9a-zA-Z\[\]\(\)\$\@._=]+$'
  ),
  password TEXT NOT NULL CHECK(TRIM(password) != ''),
  total_kills INTEGER NOT NULL CHECK(total_kills >= 0),
  total_deaths INTEGER NOT NULL CHECK(total_deaths >= 0),
  max_killing_spree INTEGER NOT NULL CHECK(max_killing_spree >= 0),
  brought_flags INTEGER NOT NULL CHECK(brought_flags >= 0),
  captured_flags INTEGER NOT NULL CHECK(captured_flags >= 0),
  dropped_flags INTEGER NOT NULL CHECK(dropped_flags >= 0),
  returned_flags INTEGER NOT NULL CHECK(returned_flags >= 0),
  head_shots INTEGER NOT NULL CHECK(head_shots >= 0),
  gungame_wins INTEGER NOT NULL CHECK(gungame_wins >= 0),
  role_id INTEGER NOT NULL CHECK(role_id >= 0 AND role_id <= 3),
  skin_id INTEGER NOT NULL CHECK(skin_id >= -1 AND skin_id <= 311),
  rank_id INTEGER NOT NULL CHECK(rank_id >= 0 AND rank_id <= 14),
  created_at TEXT NOT NULL,
  last_connection TEXT NOT NULL,
  PRIMARY KEY(id)
) STRICT;
COMMIT;