#!/bin/bash

echo "Starting entrypoint..."
echo "LagCompMode=$LagCompMode"
echo "MaxPlayers=$MaxPlayers"
echo "Port=$Port"

jq "
  .game.lag_compensation_mode = ${LagCompMode}
  | .max_players = ${MaxPlayers}
  | .network.port = ${Port}
  | .password = \"${ServerPassword}\"
  | .rcon.password = \"${RconPassword}\"
" config.json > config.tmp

mv config.tmp config.json

echo "Launching omp-server..."
exec ./omp-server