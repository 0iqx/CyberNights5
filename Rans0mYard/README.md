> [!NOTE]
> ## Rans0mYard
> **Difficulty:** Hard  
**By:** @W4L33Dx  
**Category:** Forensics  
> ### Description:
> Can you help me, I was writing something, and by mistake, I opened ransomware. I want to get my files back please.
---
#### The challenge download gave me a single file `RansomYard.ad1`. I found out through a quick search that I can open .ad1 files using FTK Imager.
![image](https://github.com/0iqx/CyberNights5/assets/165626321/e777ce72-932b-4c39-aef6-53f04a687c68)
#### I analyzed the image to find any suspicious behavior. I discovered that all files were encrypted using some form of encryption. 
![image](https://github.com/0iqx/CyberNights5/assets/165626321/fcbe3c4b-083a-40ad-9ec5-6ea7bd5bdd77)
#### I couldn't locate the ransomware initially, but then I realized, 'What if Windows Defender quarantined it?.
#### And I encountered this excellent write-up about Windows Defender Quarantine at [this link](https://blog.ry4n.org/hackthebox-ctf-confinement-write-up-a4e3b0429e30#2a9e).
#### I found `RansomYard.exe`. Using `Detect it easy` showed that it was a .NET compiled binary.
![image](https://github.com/0iqx/CyberNights5/assets/165626321/85b44adc-afc2-4099-9040-44557507ab75)
#### I opned the `RansomYard.exe` using [`IlSpy`](https://github.com/icsharpcode/ILSpy)
![image](https://github.com/0iqx/CyberNights5/assets/165626321/a16de72f-2b39-459e-abbf-d2c431a5a39f)

