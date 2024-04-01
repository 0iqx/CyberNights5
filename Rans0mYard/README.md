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
#### Check [`RandomYard.cs`](https://github.com/0iqx/CyberNights5/blob/main/Rans0mYard/RansomYard.cs) and [`RandomYardPatched.cs`](https://github.com/0iqx/CyberNights5/blob/main/Rans0mYard/RansomYard_patched.cs)
#### After decrypting all files, we encountered a curious password-protected file named `shop.zip`.
![image](https://github.com/0iqx/CyberNights5/assets/165626321/bd8fac4e-6932-4dca-9604-383203c46ca0)
#### Using [`john`](https://github.com/openwall/john).
![image](https://github.com/0iqx/CyberNights5/assets/165626321/91a9689f-e02a-4b0b-8ea1-4d2da0f0fca9)
#### While it was cracking, I revisited the description and realized that the victim was writing something when the ransomware attacked!
#### I searched for a bit and found [`this`](https://medium.com/@mahmoudsoheem/new-digital-forensics-artifact-from-windows-notepad-527645906b7b)
![image](https://github.com/0iqx/CyberNights5/assets/165626321/db2db032-b17a-4643-b586-4f4c0ac7b76d)
#### Great, we can read it, but let's make it easier to understand.
![image](https://github.com/0iqx/CyberNights5/assets/165626321/c8d655d8-529a-4825-afd1-131ec830cb64)
#### The moment I read it, [CUPP](https://github.com/Mebus/cupp) instantly came to mind.
![image](https://github.com/0iqx/CyberNights5/assets/165626321/99a7cb79-e506-4b04-bcc6-8a449dbf6f98)
#### Nice!, we cracked the password :D
![image](https://github.com/0iqx/CyberNights5/assets/165626321/b844c86e-b5e0-461e-a318-7d6617e7bb16)
#### Let's proceed to read the contents of `important.txt`
#### And thankfully it was the flag YAY!
#### FlagY{Y0u_D3t3c7_MY_R4ns0m_Am4z1ng!!!}
