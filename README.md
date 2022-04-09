# CodePathHelper
![Icon](./CodePathHelper/Resources/Icon-small.png)

Makes it super easy to share code path of Azure DevOps, or go to the file given Azure DevOps link.

## Usage:

- Download and install from [Visual Studio Extension Marketplace](https://marketplace.visualstudio.com/items?itemName=Zixuan-Wang.codepathhelper).
- Select code, right click, "Share Code Path". The generated url will be copied to your clipboard.
- Tools -> "Goto Code Path", paste the Azure DevOps link, Visual Studio will help you navigate to the file and the line.
- More options can be viewed in Tools -> Options -> Extensions -> Code Path Helper.
- To update key binding of shortcut, go to Tools -> Options -> Environment -> Keyboard, search for "Tools.ShareCodePath" and "Tools.GotoCodePath". By default they are "Ctrl+K, Ctrl+,"(Share Code Path) and "Ctrl+K, Ctrl+."(Goto Code Path).

## Attention
- Git is involved in almost every step, but please pay attention to any possible mismatches in branches: remote VS local branch, your local branch VS branch in Url shared by others, etc.

## Roadmap
- Code format when copying.
- Extend to other git service provider: GitHub, GitLab, etc.
 
## Others
Microsoft is hiring! If you are interested in any [jobs](https://careers.microsoft.com/us/en) (especially in Suzhou), feel free to contact me!

## Thanks
- Logo and icons designed by [Xiang Li](https://lxlhf940306.wixsite.com/mysite).
- [Mads Kristensen](https://github.com/madskristensen) who provided great tutorials of visual studio extensions development.
