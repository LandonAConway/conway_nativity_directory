   M S B A M L   `   `   `  ���� T  PConway Nativity Directory, Version=1.2.1.0, Culture=neutral, PublicKeyToken=nullE'clr-namespace:Conway_Nativity_DirectoryConway_Nativity_Directory  +    %Conway_Nativity_Directory.AddNativity   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     9local'clr-namespace:Conway_Nativity_Directory       � Title$  Add Nativity��$	��430��$	��500�� � WindowStartupLocation$ CenterScreen=� � 
ResizeMode$ NoResize=� � ShowInTaskbar .   � Topmost . .���  � RowDefinitions �� $
��Auto ��� .+��� $��
10,10,10,5q�.+���  �� Orientation$ 
Horizontal=�.+��� $	��Id:��-   'Conway_Nativity_Directory.CustomTextbox  -     Name$ 	idTextBox��$��20��   	RegexMask$ [^0-9]+��$	��200���� $ 
Horizontal=�.+��� $��Title:���� -   $��titleTextBox��$��20��$	��200���� $ 
Horizontal=�.+��� $��Origin:���� -   $��originTextBox��$��20��$	��200���� $ 
Horizontal=�.+��� $��	Acquired:��  -   $ acquiredTextBox��$��20��$ [^0-9]+��$	��200���� $ 
Horizontal=�.+��� $��From:���� -   $��fromTextBox��$��20��$	��200���� $ 
Horizontal=�.+��� $��Cost:��  -   $ costTextBox��$��20��	   NumericOnly	 . $	��200���� $ 
Horizontal=�.+��� $��	Location:���� -   $��locationTextBox��$��20��$	��200���� $��Description:��$��
5,15,10,10q��� -   
 ��Name$
 descriptionTextBox��$	��100��$	��5,0q� �� AcceptsReturn .  �� 
AcceptsTab .  �� TextWrapping$
 Wrap=��� $��1��$��10,15,10,10q�$ 
Horizontal=�$��RightToLeft=�$��Bottom=�.+��� -	   $��cancelButton��$	��100��$��Cancel��$��24���� -
   $��okButton��$	��100��$��Ok��$��24��$��10,0,0,0q��� $��1��$��10,15,10,10q�$ 
Horizontal=�$��LeftToRight=�$��Bottom=�.+��� -   $��parseButton��$	��100��$��Parse��$��24��