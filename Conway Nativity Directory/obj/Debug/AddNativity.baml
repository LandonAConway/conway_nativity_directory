   M S B A M L   `   `   `  ����T  PConway Nativity Directory, Version=2.2.3.0, Culture=neutral, PublicKeyToken=nullE'clr-namespace:Conway_Nativity_DirectoryConway_Nativity_Directory  5   	   +    %Conway_Nativity_Directory.AddNativity   5   	   ^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   9local'clr-namespace:Conway_Nativity_Directory     5   	     � Title$  Add Nativity��5   	   $	��430��6   $	��500��6+    � WindowStartupLocation$ CenterScreen=�67    � 
ResizeMode$ NoResize=�6\    � ShowInTaskbar .   � Topmost . .��5
      �  � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      .+�5   
   �� $��
10,10,10,5q�6   .+�5      ��  �� Orientation$ 
Horizontal=�6   .+�5      �� $	��Id:��6   -   'Conway_Nativity_Directory.MaskedTextBox  5      -   5          Name$ 	idTextBox��5   &   $��20��69      	RegexMask$ [^0-9]+��6E   $	��200��6Y   5      �� 5      $ 
Horizontal=�6   .+�5      �� $��Title:��6   �� 5      -   5        $��titleTextBox��5      $��20��6.   $	��200��6:   5      �� 5      $ 
Horizontal=�6   .+�5      �� $��Origin:��6   �� 5      -   5        $��originTextBox��5      $��20��6/   $	��200��6;   5      �� 5      $ 
Horizontal=�6   .+�5      �� $��	Acquired:��6     5      -   5        $ acquiredTextBox��5   &   $��20��6?   $ [^0-9]+��6K   $	��200��6_   5      �� 5       $ 
Horizontal=�6   .+�5!      �� $��From:��6   �� 5"      -   5        $��fromTextBox��5"      $��20��6-   $	��200��69   5#      �� 5$      $ 
Horizontal=�6   .+�5%      �� $��Cost:��6     5&      -   5        $ costTextBox��5&   &   $��20��6;   	   NumericOnly	 . $	��200��6Z   5'      �� 5(      $ 
Horizontal=�6   .+�5)      �� $��	Location:��6   �� 5*      -   5        $��locationTextBox��5*      $��20��61   $	��200��6=   5+      �� 5,      $��Description:��6   $��
5,15,10,10q�6,   �� 5-      -   5        
 ��Name$
 descriptionTextBox��5-      $	��100��62   $	��5,0q�6?    �� AcceptsReturn .  �� 
AcceptsTab .  �� TextWrapping$
 Wrap=�6s   5.      �� 5/   
   $��1��6   $��10,15,10,10q�6"   $ 
Horizontal=�67   $��RightToLeft=�6P   $��Bottom=�6l   .+�50      �� -	   5        $��cancelButton��50      $	��100��6)   $��Cancel��65   $��24��6F   6R   �� 51      -
   5        $��okButton��51      $	��100��6%   $��Ok��61   $��24��6>   $��10,0,0,0q�6J   6\   52      �� 53   
   $��1��6   $��10,15,10,10q�6"   $ 
Horizontal=�67   $��LeftToRight=�6P   $��Bottom=�6l   .+�54      �� -   5        $��parseButton��54      $	��100��6(   $��Parse��64   $��24��6D   6P   55      56      57      