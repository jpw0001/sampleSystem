# sampleSystem #
Sample set of nodes to read and write messages to a kafka topic
Create small simple communications lib. Hooks are from the consume message.

Steps:
---
1. open Terminal
2. start up docker kafka using 'docker compose up -d'
3. create topic 'quickstart' using 'create-topic.sh'
4. build the sampleNode by 'dotnet build ./sampleNode'
5. build the otherNode by 'dotnet build ./otherNode'
6. need to send a message to 'quickstart' with value 'apple'
7. observe sampleNode get 'apple' and dispatch 'orange', and then otherNode get 'orange' (and 'apple') and dispatch 'grape'.


To just listen to messages on the topic use 'read-messages.sh'

Some to dos:
---
* create a book lib
* generate transport message lib
