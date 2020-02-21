# DenaryBinaryHexOctalGUIVer
Converts from one numeric base (binary, hexadecimal etc.) to another with a GUI made with WPF.

**Installing:**
Simply go to the releases tab and find the latest version. Download the .EXE from there and run it.

**Building from Source:**
Download the repository and open the .sln file. Then, in visual studio, select Release (Any CPU), and then Build it. In your bin folder, go to Release and you will find your .exe there.

**ROADMAP:**
- Allow support for comma separated values and spaces
- Decimal support?
- Re-order UI
- Keyboard shortcuts
- Do something about bases greater than 36
  - Potentially add option to use (10),(11),(12) instead of A, B, C?
- Realtime conversion? (convert as the user is typing)
- Prevent the entering of invalid characters instead of just telling the user off and wiping both the input and output fields
- Basic binary operators such as AND, OR, NOT, and left shift and right shift operators?
- Support for non Windows platforms (Avalonia?)
