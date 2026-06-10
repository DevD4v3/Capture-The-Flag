#!/bin/bash

echo "Starting entrypoint..."
echo "Port=$Port"
echo "MaxPlayers=$MaxPlayers"

jq "
  .game.lag_compensation_mode = ${LagCompMode}
  | .max_players = ${MaxPlayers}
  | .network.port = ${Port}
  | .password = \"${ServerPassword}\"
" config.json > config.tmp

mv config.tmp config.json

echo "Launching omp-server..."
exec ./omp-server