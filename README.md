# VerusMiningGui
A Verus Mining GUI for nheqminer. Tested with VerusHash 2.0 nheqminer 0.7.1 Windows build.
    -This software is an adaptation of VerusMiningGui, forked from the original git at https://github.com/moisesmcardona/VerusMiningGui

This software is written in Visual Basic .NET using Visual Studio 2015. Compiled for .NET Framework 4.6.1

To use:
1. Rirst download the VerusHash "nheqminer" from https://github.com/VerusCoin/nheqminer/releases and place the executable "nheqminer.exe" inside a folder called "nheqminer"
2. Download the latest release of Verus EZ CPU Pool Miner from https://github.com/Nine27/VerusEZCPUPoolMiner/releases and place the executable "VerusEZCPUPoolMiner.exe" in the same "nheqminer" folder
3. Run the "VerusEZCPUPoolMiner.exe" binary and enter a valid Verus (VRSC) wallet you own and the pool server as poolurl:port, then click Start Mining!

If you have any trouble, join the Verus discord at https://discord.gg/VRKMP2S and ask for help!

- If the output of the miner doesn't show realtime, choose the checkbox "Show CMD Window" to see realtime mining output.
- You can minimize the GUI and it will keep mining in the System Tray. Double click it to show the software.

That's all. Enjoy!

# Changelog
v0.4 (01/18/2019) </br>
-Forked from original repo by moisesmcardona</br>
-Rebranded for Verus Coin</br>
-Adapted for VerusHash nheqminer 0.7.1</br>
--Remove GPU Settings</br>
--Change CPU Settings and defaults</br>
--Change layout and GUI</br>
--Change folder association </br>
--Change internal call to nheqminer</br>
-Renamed to Verus EZ CPU Pool Miner</br>

v0.3 (11/4/2016) </br>
-Added Stop Mining button </br>
-Performance improvements

v0.2b (11/4/2016)</br>
-Fixed Threads and Blocks not being set to default values when Threads and Blocks are set to 0 in the Tuning Window</br>
-Added a message in the Tuning Window that 0 means default values

v0.2 (11/4/2016)</br>
-Added Compatibility Mode for Nvidia cards (-cv 1)</br>
-Added Threads and Blocks tuning for Nvidia cards (-ct and -cb)</br>
-Now using the nheqminer.exe executable</br>
-EU and USA server option removed in favor for a user-specified server and port. Currently defaults to Nicehash Verus pool

v0.1</br>
-Initial Releas
