-- name: CreatePlayer
INSERT INTO players 
(
	name,
	password,
	total_kills,
	total_deaths,
	max_killing_spree,
	brought_flags,
	captured_flags,
	dropped_flags,
	returned_flags,
	head_shots,
	gungame_wins,
	role_id,
	skin_id,
	rank_id,
	created_at,
	last_connection
)
VALUES (
	$name,
	$password,
	$total_kills,
	$total_deaths,
	$max_killing_spree,
	$brought_flags,
	$captured_flags,
	$dropped_flags,
	$returned_flags,
	$head_shots,
	$gungame_wins,
	$role_id,
	$skin_id,
	$rank_id,
	$created_at,
	$last_connection
);
select last_insert_rowid()

-- name: UpdatePlayerName
UPDATE players SET name = $name WHERE id = $id

-- name: GetPlayerByName
SELECT *
FROM players
WHERE name = $name

-- name: PlayerExists
SELECT 1
FROM players
WHERE name = $name

-- name: GetTopPlayersByMaxKillingSpree
SELECT 
name,
max_killing_spree
FROM players
WHERE max_killing_spree >= $required_max_killing_spree
ORDER BY max_killing_spree DESC 
LIMIT $max_players

-- name: GetTopPlayersByTotalKills
SELECT 
name,
total_kills,
rank_id
FROM players
WHERE total_kills >= $required_total_kills
ORDER BY total_kills DESC 
LIMIT $max_players