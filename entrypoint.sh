#!/bin/bash

jq "
  .game.lag_compensation_mode = ${LagCompMode}
  | .max_players = ${MaxPlayers}
  | .network.port = ${Port}
  | .password = \"${ServerPassword}\"
" config.json > config.tmp

mv config.tmp config.json

exec ./omp-server