# DonnerTanks!

## Overview

DonnerTanks! is a arcade arena tank brawler with micropayments. It is based on [Unity's Tanks! Tutorial](https://assetstore.unity.com/packages/essentials/tutorial-projects/tanks-tutorial-46209) and uses [lnd](https://github.com/lightningnetwork/lnd) for payments. Each player controls a tank and tries to defeat his opponents, whilst beeing aided or hindered by outside payments.


## Running DonnerTanks!

- Download the [build](https://github.com/donnerlab1/donnertanks/releases)
- Edit the donnertanks.conf file in DonnerTanks_Data/Resources/ to fit your lnd node
- Copy your tls.cert and admin.macaroon to DonnerTanks_Data/Resources/
- Run the Game
- Spectators can request lightning-invoices by navigating to http://your_networkIp:8079/tanks?

### Building DonnerTanks!

- your can build DonnerTanks with Unity 2017.4
- see [DonnerUnity](https://github.com/donnerlab1/donnerunity/) for further information
