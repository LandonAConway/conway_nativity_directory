   M S B A M L   `   `   `  ����T  PConway Nativity Directory, Version=2.2.3.0, Culture=neutral, PublicKeyToken=nullE'clr-namespace:Conway_Nativity_DirectoryConway_Nativity_Directory  5   	   +    %Conway_Nativity_Directory.InputDialog   5   	   ^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   9local'clr-namespace:Conway_Nativity_Directory     5   	   -   5          � Title$  Conway Nativity Directory��5   	   $	��110��6+   $	��300��68    � 
ResizeMode$ NoResize=�6D    � ShowInTaskbar .   � Topmost .  � WindowStartupLocation$ CenterScreen=�5	   	    J� CommandBindings 5
      )  #System.Windows.Input.CommandBinding  5   
   -   5           Command$ local:CustomCommands.Ok��5      6;     5   
   -   5        $&  local:CustomCommands.CloseWindow��5      6D    J� InputBindings 5      �� 5   
   
 �� Key$ Return��6   	 �� Command$	 local:CustomCommands.Ok��6"   �� 5   
   $	 Esc��6   $&	  local:CustomCommands.CloseWindow��6   .��5      �� .+�5   
   �� -   5        
 ��Name$
 ResponseTextBox��5      $��10,10,10,10q�6+   $��20��6@   �� 5   
   -   5        $	��100��5      $��20��6   $��Ok��6)   66   5      5      