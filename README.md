# Defend Marsai
A fireemblem like strategy game. 

## Design 
* Units will have fatigue. Each battle or action (not movement) will increase their fatigue. Support classes can reduce this for other units. 
* Weapons will have durability and classes. 
* I don't know if I'll have a weapons triangle, or some weapons will just be better than others (or the same all around). 
* Units can have sub-classes with a max of 2 (can normally be a swordsman but can learn to draw a bow, giving 2 ways to attack). 
* I want to have something to do between battles. FE is fun, but it can be repetitive to do batte after battle. I'd like to add 
more strategy by making you choose which battles on the main map (where to defend) and be responsible for villages' wellbeing. You'll
also need enough supplies for your army (food, boots, weapons). 

## Enemy Design
### Easy
Start with 'Easy' AI where it chooses a unit randomly within range.
### Medium
AI picks the weakest unit to attack, priorizing finishing a unit off.
### Hard 
AI uses a deep search to 'lookahead' at the possible moves and chooses the best path for its advantage. 
### Impossible
AI model is trained over generations by other AIs. 