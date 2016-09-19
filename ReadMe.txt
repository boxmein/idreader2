idreader2
=========

A program that sits in the foreground, and with zero interaction, 
collects ID numbers from ID cards connected to this computer. 

Handles many readers and many cards concurrently and efficiently.
Logs to a timestamped file.

Issues
------

- [ ] GUI is ugly
- [ ] Data logging isn't perfect
- [ ] Code looks like a mess
  - [ ] Refactor error handling, smart card logic into smaller 
        chunks.
  - [ ] Wait instead of polling
