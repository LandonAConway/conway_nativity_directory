   M S B A M L   `   `   `  ����T  PConway Nativity Directory, Version=2.2.3.0, Culture=neutral, PublicKeyToken=nullE'clr-namespace:Conway_Nativity_DirectoryConway_Nativity_Directory  5   	   > :Api, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullb<clr-namespace:ConwayNativityDirectory.PluginApi;assembly=Api!ConwayNativityDirectory.PluginApi 5   	   *    $Conway_Nativity_Directory.MainWindow   5   	   ^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   9local'clr-namespace:Conway_Nativity_Directory     5   	   Mpapi<clr-namespace:ConwayNativityDirectory.PluginApi;assembly=Api     5   	     � Title$  Conway Nativity Directory��5	   	   $	��450��6+   $	��800��68    � WindowState$ 	Maximized=�6D    � 	Resources 5
      7   1Conway_Nativity_Directory.BooleanInverseConverter   booleanInverseConverter<   6Conway_Nativity_Directory.BooleanToVisibilityConverter   booleanToVisibilityConverter@   :Conway_Nativity_Directory.SortingTypeToVisibilityConverter   SortingType  idVisibility  titleVisibility  originVisibility  acquiredVisibility  fromVisibility  costVisibility  locationVisibility>   8Conway_Nativity_Directory.AllowEditingTimeStampConverter "	 allowEditingTimestampConverter 
 	ClearPath��%�  &	        &	      &	 &     &	 G     &	 k     &	 �     &	 �     &	 �     &	 �     &		 $    &	
 7      5   
   6(     5   
   6-     5   
   $ Id=�61   6B     5   
   $ Title=�61   6E     5   
   $ Origin=�61   6F     5   
   $ Acquired=�61   6H     5   
   $
 From=�61   6D     5   
   $
 Cost=�61   6D     5   
   $ Location=�61   6H     5   
   6/   B�5   
   M0,0 L1,1 M1,0 L0,1�6>   5       J� CommandBindings 5      )  #System.Windows.Input.CommandBinding  5   
   -   5           Command$ local:CustomCommands.New��5      6<     5   
   -   5        $ local:CustomCommands.Open��5      6=     5   
   -   5        $  local:CustomCommands.Close��5      6>     5   
   -   5        $ local:CustomCommands.Save��5      6=     5   
   -   5        $! local:CustomCommands.SaveAs��5      6?     5   
   -   5        $$ local:CustomCommands.SaveACopy��5      6B     5   
   -   5        $  local:CustomCommands.Login��5      6>     5   
   -   5        $! local:CustomCommands.Logout��5      6?     5    
   -	   5        $ local:CustomCommands.Undo��5       6=     5!   
   -
   5        $ local:CustomCommands.Redo��5!      6=     5"   
   -   5        $+ %local:CustomCommands.RotateImageRight��5"      6I     5#   
   -   5        $* $local:CustomCommands.RotateImageLeft��5#      6H     5$   
   -   5        $) #local:CustomCommands.AdvancedSearch��5$      6G     5%   
   -   5        $ local:CustomCommands.Find��5%      6=     5&   
   -   5        $ local:CustomCommands.GoTo��5&      6=     5'   
   -   5        $ local:CustomCommands.Add��5'      6<     5(   
   -   5        $! local:CustomCommands.Remove��5(      6?     5)   
   -   5        $! local:CustomCommands.Delete��5)      6?     5*   
   -   5        $$ local:CustomCommands.SelectAll��5*      6B     5+   
   -   5        $% local:CustomCommands.SelectNone��5+      6C     5,   
   -   5        $' !local:CustomCommands.SelectInvert��5,      6E     5-   
   -   5        $/ )local:CustomCommands.CreateTagForSelected��5-      6M     5.   
   -   5        $0 *local:CustomCommands.RemoveTagFromSelected��5.      6N     5/   
   -   5        $> 8local:CustomCommands.CreateGeographicalOriginForSelected��5/      6\     50   
   -   5        $? 9local:CustomCommands.RemoveGeographicalOriginFromSelected��50      6]     51   
   -   5        $= 7local:CustomCommands.OrganizeTagsAndGeographicalOrigins��51      6[     52   
   -   5        $# local:CustomCommands.Commands��52      6A     53   
   -   5        $ local:CustomCommands.Exit��53      6=    J� InputBindings 55      �� 56   
   
 �� Key$ N��6    �� 	Modifiers$
 Ctrlf�6   	 �� Command$	 local:CustomCommands.New��6.   �� 57   
   $ O��6   $
 Ctrlf�6   $	 local:CustomCommands.Open��6.   �� 58   
   $ W��6   $
 Ctrlf�6   $ 	 local:CustomCommands.Close��6.   �� 59   
   $ S��6   $
 Ctrlf�6   $	 local:CustomCommands.Save��6.   �� 5:   
   $ S��6   $ 
Ctrl+Shiftf�6   $!	 local:CustomCommands.SaveAs��64   �� 5;   
   $ S��6   $ Ctrl+Shift+Altf�6   $$	 local:CustomCommands.SaveACopy��68   �� 5<   
   $ L��6   $
 Ctrlf�6   $ 	 local:CustomCommands.Login��6.   �� 5=   
   $ L��6   $	 Altf�6   $!	 local:CustomCommands.Logout��6-   �� 5>   
   $ Z��6   $
 Ctrlf�6   $	 local:CustomCommands.Undo��6.   �� 5?   
   $ Z��6   $ 
Ctrl+Shiftf�6   $	 local:CustomCommands.Redo��64   �� 5@   
   $ R��6   $ 
Ctrl+Shiftf�6   $+	 %local:CustomCommands.RotateImageRight��64   �� 5A   
   $ L��6   $ 
Ctrl+Shiftf�6   $*	 $local:CustomCommands.RotateImageLeft��64   �� 5B   
   $ S��6   $ 	Shift+Altf�6   $)	 #local:CustomCommands.AdvancedSearch��63   �� 5C   
   $ F��6   $
 Ctrlf�6   $	 local:CustomCommands.Find��6.   �� 5D   
   $ G��6   $
 Ctrlf�6   $	 local:CustomCommands.GoTo��6.   �� 5E   
   $ A��6   $ 	Shift+Altf�6   $	 local:CustomCommands.Add��63   �� 5F   
   $ R��6   $ 	Shift+Altf�6   $!	 local:CustomCommands.Remove��63   �� 5G   
   $ Delete��6   $!	 local:CustomCommands.Delete��6"   �� 5H   
   $ A��6   $ 
Ctrl+Shiftf�6   $$	 local:CustomCommands.SelectAll��64   �� 5I   
   $ N��6   $ 
Ctrl+Shiftf�6   $%	 local:CustomCommands.SelectNone��64   �� 5J   
   $ I��6   $ 
Ctrl+Shiftf�6   $'	 !local:CustomCommands.SelectInvert��64   �� 5K   
   $ T��6   $ 
Ctrl+Shiftf�6   $/	 )local:CustomCommands.CreateTagForSelected��64   �� 5L   
   $ T��6   $ Ctrl+Altf�6   $0	 *local:CustomCommands.RemoveTagFromSelected��62   �� 5M   
   $ G��6   $ 
Ctrl+Shiftf�6   $>	 8local:CustomCommands.CreateGeographicalOriginForSelected��64   �� 5N   
   $ G��6   $ Ctrl+Altf�6   $?	 9local:CustomCommands.RemoveGeographicalOriginFromSelected��62   �� 5O   
   $ O��6   $ 
Ctrl+Shiftf�6   $=	 7local:CustomCommands.OrganizeTagsAndGeographicalOrigins��64   �� 5P   
   $ F2��6   $ Shiftf�6   $#	 local:CustomCommands.Commands��60   �� 5Q   
   $ F1��6   $	 Altf�6   $	 local:CustomCommands.Exit��6.   .��5S      � 
 � RowDefinitions
 5T   
   �� 5U      $
��Auto �6   �� 5V      �� 5W      $
��Auto �6   .+�5Y   
   o� $��24��6   $��0��6    5Z       �� 
TargetType �� Triggers ^� 
Conditions �� Binding �� Path �� RelativeSource �� AncestorType*   $Conway_Nativity_Directory.MainWindow �� Value �� Property �� Value  collapsableSep��%1  'm�       &	 5    �� 5[      " m�6    5\      ^� 5]       5^      �� 5_   "    6,   �� $  LoginSettings.AllowEditing� �� "  	 False5`   j   �� 5a   "    6,   �� $	 Tag� �� *Self+ Collapsable6o   0�5c      �� 5d   "    � y�$ 	Collapsed=�6?   5f      5h      �� 5i      " ��6    5j      ^� 5k       5l      �� 5m   "    6,   �� $  LoginSettings.AllowEditing� �� "  	 False5n   j   0�5p      �� 5q   "    � y�$ 	Collapsed=�6?   5s      5u      5v      .@�5w      m� -   5        $��fileMenuItem��5w   %   $
��File��6   .@�5x      m� -   5        $��newMenuItem��5x      $	��New��6.   
 � Tag$ Collapsable��5y   5    m� InputGestureText$ Ctrl+N��6G   m� 5z      -   5        $��openMenuItem��5z      $��Open...��6/   $ Collapsable��5{   6   $ Ctrl+O��6H   �� 5|      #���  6   m� 5}      -    5        $��closeMenuItem��5}      $��Close��60   $ Collapsable��5~   7   $ Ctrl+W��6I   m� 5      -!   5        $��saveMenuItem��5      $
��Save��6/   $ Collapsable��5�   6   $ Ctrl+S��6H   m� 5�      -"   5        $��saveAsMenuItem��5�      $��
Save As...��61   $ Collapsable��5�   8   $ Ctrl+Shift+S��6J   m� 5�      -#   5        $��saveACopyMenuItem��5�      $��Save a Copy...��64   $ Collapsable��5�   ;   $ Ctrl+Shift+Alt+S��6M   �� 5�      #���  6   m� 5�      -$   5        $��scriptsMenuItem��5�      $��Scripts��62   .@�5�      m� -%   5        $��scriptsManagerMenuItem��5�      $��Scripts Manager...��6=   6Y   5�      �� 5�      #���  6   m� 5�      -&   5        $��importMenuItem��5�      $ Collapsable��61   $��Import��6C   .@�5�      m� -'   5        $��parseNarrativesMenuItem��5�      $��
Narratives��6>   6R   5�      m� 5�      -(   5        $��exportMenuItem��5�      $ Collapsable��61   $��Export��6C   .@�5�      m� -)   5        $��exportNarrativesMenuItem��5�      $��
Narratives��6?   6S   5�      �� 5�      #���  6   m� 5�      -*   5        $��loginMenuItem��5�      $��Login...��60   $ Ctrl+L��5�   7   m� 5�      -+   5        $��logoutMenuItem��5�      $��Logout��61   $ Alt+L��5�   8   ��5�      �� 5�      " m�6    5�      5�      �� 5�      m� 5�      -,   5        $��exitMenuItem��5�      $
��Exit��6/   $ Alt+F1��5�   6   5�      m� 5�      --   5        $��projectMenuItem��5�      $��Project��6.   $ Collapsable��6?   .@�5�      m� -.   5        $��settingsMenuItem��5�      $��Settings...��63   5�      5�      m� 5�      -/   5        $��editMenuItem��5�      $
��Edit��6+   $ Collapsable��69   .@�5�      m� -0   5        $��undoMenuItem��5�      $
��Undo��6/   $ Ctrl+Z��5�   6   m� 5�      -1   5        $��redoMenuItem��5�      $
��Redo��6/   $ Ctrl+Shift+Z��5�   6   �� 5�      m� 5�      -2   5        $��imageMenuItem��5�      $��Image Tools��60   .@�5�      m� -3   5        $��assignImagesByIdMenuItem��5�      $��Assign Images by Id...��6?   5�      m� 5�      -4   5        $��removeImagesMenuItem��5�      $��Remove Images...��6;   5�      �� 5�      m� 5�      -5   5        $��rotationMenuItem��5�      $��Rotation��67   .@�5�      m� -6   5        $��rotateRightMenuItem��5�   #   $��Rotate Right��6>   $ Ctrl+Shift+R��5�   A   m� 5�      -7   5        $��rotateLeftMenuItem��5�   #   $��Rotate Left��6=   $ Ctrl+Shift+L��5�   @   5�      m� 5�      -8   5        $��embeddImagesMenuItem��5�      $��Embedd Images��6;   5�      m� 5�      -9   5        $��relinkImagesMenuItem��5�      $��Relink Images...��6;   5�      5�      m� 5�      -:   5        $��taggingMenuItem��5�      $��Tagging��62   .@�5�      m� -;   5        $(��"organizeTagsAndGeographicalOrigins��5�      $*��$Organize Tags & Geographical Origins��6I   $ Ctrl+Shift+O��5�   R   m� 5�      -<   5        $��addTagToSelectionMenuItem��5�      $��Add Tag to Selection...��6@   $ Ctrl+Shift+T��5�   G   m� 5�      -=   5        $$��removeTagFromSelectionMenuItem��5�      $"��Remove Tag from Selection...��6E   $ 
Ctrl+Alt+T��5�   L   m� 5�      ->   5        $.��(addGeographicalOriginToSelectionMenuItem��5�      $-��'Add Geographical Origin to Selection...��6O   $ Ctrl+Shift+G��5�   V   m� 5�      -?   5        $3��-removeGeographicalOriginFromSelectionMenuItem��5�      $2��,Remove Geographical Origin from Selection...��6T   $ 
Ctrl+Alt+G��5�   [   5�      m� 5�      -@   5        $��namingMenuItem��5�      $��Naming��61   .@�5�      m� -A   5        $��uppercaseMenuItem��5�      $��Uppercase...��68   6N   m� 5�      -B   5        $��lowercaseMenuItem��5�      $��Lowercase...��68   6N   m� 5�      -C   5        $��fixCapitalizationMenuItem��5�      $��Fix Capitalization...��6@   6_   m� 5�      -D   5        $��replaceTextMenuItem��5�      $��Replace Text...��6:   6S   m� 5�      -E   5        $��replaceTextRegexMenuItem��5�      $��Replace Text (Regex)...��6?   6`   5�      �� 5�      m� 5�      -F   5        $��preferencesMenuItem��5�      $��Preferences��66   5�      5�      m� 5�      -G   5        $��nativitiesMenuItem��5�      $��
Nativities��61   $ Collapsable��6E   .@�5�      m� -H   5        $��addNativityMenuItem��5�      $��Add...��66   $ Shift+Alt+A��5�   =   m� 5�      -I   5        $��removeNativityMenuItem��5�      $��Remove��69   $ Shift+Alt+R��5�   @   �� 5�      m� 5�      $��	Selection��6   .@�5�      m� $��Select��6   .@�5�      m� -J   5        $��selectAllMenuItem��5�   #   $	��All��6<   $ Ctrl+Shift+A��5�   D   m� 5�      -K   5        $��selectNoneMenuItem��5�   #   $
��None��6=   $ Ctrl+Shift+N��5�   E   m� 5�      -L   5        $��selectInvertMenuItem��5�   #   $��Invert��6?   $ Ctrl+Shift+I��5�   G   5�      5�      5�      m� 5�      -M   5        $��searchMenuItem��5�      $��Search��6-   .@�5�      m� -N   5        $��findMenuItem��5�      $��Find...��6/   $ Ctrl+F��5�   6   m� 5�      -O   5        $��goToMenuItem��5�      $��Go To...��6/   $ Ctrl+G��5�   6   m� 5�      -P   5        $��advancedSearchMenuItem��5�      $��Advanced Search...��69   $ Shift+Alt+S��5�   @   �� 5�      #���  6   m� 5�      -Q   5        $��searchToolsMenuItem��5�      $��Tools��66   $ Collapsable��6E   .@�5�      m� -R   5        $$��findMissingNativityIdsMenuItem��5�      $"��Find Missing Nativity Ids...��6E   5�      m� 5�      -S   5        $!��findNativitiesWithoutImages��5�      $'��!Find Nativities Without Images...��6B   5�      �� 5�      $y�	Collapsed=�6    m� 5�      -T   5        $��highlightToolMenuItem��5�      $��Highlight Tool...��6<   $y�	Collapsed=�5�   C   5�      5�      m� 5�      -U   5        $��viewMenuItem��5�      $
��View��6+   $ Collapsable��69   .@�5�      m� -V   5        $��workspacesMenuItem��5�      $��
Workspaces��65   �� 5�      m� 5�      -W   5        $��viewIdColumnMenuItem��5�      $��Id��67    m� IsCheckable . 5�   @   m� 5�      -X   5        $��viewTitleColumnMenuItem��5�      $��Title��6:    . 5�   @   m� 5�      -Y   5        $��viewOriginColumnMenuItem��5�      $��Origin��6;    . 5�   @   m� 5�      -Z   5        $ ��viewAcquiredColumnMenuItem��5�      $��Acquired��6=    . 5�   @   m� 5      -[   5        $��viewFromColumnMenuItem��5      $
��From��69    . 5  @   m� 5     -\   5        $��viewCostColumnMenuItem��5     $
��Cost��69    . 5  @   m� 5     -]   5        $ ��viewLocationColumnMenuItem��5     $��Location��6=    . 5  @   m� 5     -^   5        $��viewTagsColumnMenuItem��5     $
��Tags��69    . 5  @   m� 5     -_   5        $+��%viewGeographicalOriginsColumnMenuItem��5     $��Geographical Origins��6H    . 5	  @   �� 5
     5     m� 5     -`   5        $��pluginsMenuItem��5     $��Plugins��6.   $ Collapsable��6?   .@�5     m� -a   5        $��managePluginsMenuItem��5     $��	Manage...��68   6K   5     m� 5     -b   5        $��helpMenuItem��5     $
��Help��6+   .@�5     m� -c   5        $��Changelogs...��5     5     �� 5     m� 5     -d   5        $��About...��5     5     5     5     � 5  
   -e   5        $��projectGrid��5     $��1��6"   .+�5     � -f   5        $��workspaceViewMode_custom��5     $y�	Collapsed=�63   .+�5     �� -g   5        $-��'workspaceViewMode_custom_contentControl��5  !   5     � 5     -h   5        $��workspaceViewMode_panes��5     
 5     �� 5     $
��Auto �6$   �� 5     �� 5     $
��Auto �6$    � ColumnDefinitions 5!     �� 5"     �� 5#     $
��Auto �6'   �� 5$     $
��0.5* �6'   .+�5'     �� $��1��6   $��0��6)   $��1��66   $��3��6F   $��2.5,0q�6W   $��Stretch=�6f   � 5(     -i   5         �Name$ viewPanesGrid��5(     $��0��6.   $��3��6;   $��5q�6L   .+�5)     $   Conway_Nativity_Directory.Pane  -j   5          Name$ mainViewPane��5)  !      WorkspaceKey$
 main��67   .@�5*     0  *ConwayNativityDirectory.PluginApi.PaneItem  $��
Nativities��6(   
   Key$ nativity��6<   .��5,     � 
 5-  "   �� 5.  &   $
��Auto �64   �� 5/  &   �� 50  &   $
��Auto �64   .+�54  "   � $��0��6'   $��0��64   $��22��6D   $��	10,10,5,5q�6P   .+�55  &   �� -k   5        $��searchTextBox��55  .   6C   �� 56  &   -l   5        ._�57  *   �� $��Advanced Search��64   $��Center=�6K   $	��5,0q�6f   ��58  *   �� 59  .   " ��64   .�5:  2   ��  � ��# Z�6O   �� 5;  2    � y�$ 	Collapsed=�6O    5<  2   P� 5=  6    P� Property �@J�IsMouseOver P� Value . . �5>  :   ��  � �� �	#FF98CCF56W   5?  7   �� 5@  6    �� Binding 6B   �� $ AdvancedSearchIsActive� �� "    �� Value  True5A  j   .a�5B  :   ��  � y�$ Visible=�6W   5C  7   5E  /   5G  '   �� 5H  &   $��1q�61   ! �� Orientation$! 
Horizontal=�6<   $��RightToLeft=�6U   .+�5I  *   �� -m   5        $��advancedSearchButton��5I  1   
������ ��6M   �� $ ActualHeight� �� *Self+��5J     Z� ��5K  .   F� 5L  2   $��1��67   
�������" �� Stretch$" Uniform=�6Y   O����        ���-     �    @B ���- @B �    �� ���- �� �    ��- ���- ��- 	$��4q�6�   ��5N  .   �� 5O  2   " ��68   .�5P  6   ��  � y�$ 	Collapsed=�6S    5Q  6   �� 5R  :    6F   �� $
 Text�# �� ElementName$# searchTextBox��   6   .a�5S  >   ��  � y�$ Visible=�6[   5T  ;   �� 5U  :    6F   �� $ AdvancedSearchIsActive� �� "    True5V  v   .a�5W  >   ��  � y�$ 	Collapsed=�6[   5X  ;   5Z  3   5\  +   �� 5]  *   -n   5        $��clearSeachButton��5]  1   
������ ��6I   �� $ ActualHeight� �� *Self+��5^     Z� ��5_  .   F� 5`  2   $��1��67   
�������$" Uniform=�6Y   #��� 
 6k   $��4q�6�   ��5b  .   �� 5c  2   " ��68    5d  6   �� 5e  :    6F   �� $
 Text�$# searchTextBox��   6   .a�5f  >   ��  � y�$ 	Collapsed=�6[   5g  ;   �� 5h  :    6F   �� $ AdvancedSearchIsActive� �� "    True5i  r   .a�5j  >   ��  � y�$ Visible=�6[   5k  ;   5m  3   5o  +   5p  '   5q  #   �� 5r  "   -o   5        $��nativityListView��5r  +   $��1��6C   $��0��6P   $��10,5,5,5q�6`   $ %� IsVirtualizing$ . ��5t  &   �� 5u  *   .R�5v  .   >	   8Conway_Nativity_Directory.NativityVirtualizingStackPanel	  5w  +   ��5y  &   �� 5z  *   " ��60    5{  .   P� 5|  2    �@��
IsSelected . . �5}  6   ��  � �� �	#4C008BFF6S   �� 5~  6    � �� �	#FF4689FF6S   �� 5  6    � �� �	#7F008BFF6T   5�  3   P� 5�  2    �@J�IsMouseOver . . �5�  6   ��  � �� �	#19008BFF6S   �� 5�  6    � �� �	#4C008BFF6T   5�  3   �� 5�  2    6>   ��  �� "  $  MultipleNativitiesSelected�  True5�  S   .a�5�  6   ��  �@�ContextMenu  multiNativityContextMenu# �  6T   5�  3   .�5�  .   ��  �@�ContextMenu  singleNativityContextMenu# �  6L   �� 5�  .    �@��
IsSelected 6K   �� *
IsSelected+% �� Mode$% TwoWay=�& �� UpdateSourceTrigger$& PropertyChanged=��� 5�  .    � �� 5�  2   �� 5�  6   ' �� 
TargetType"' ��6F   .R�5�  :   �� ( �� Padding#( z��6A   #��z��6e   #��z��5�  9   #��z��6e   ) J� SnapsToDevicePixels) . ._�5�  >   �� #��z��5�  =   #��z��5�  =   #) z) 5�  =   5�  ;   5�  7   5�  /   5�  +    5�  &   -
   'Conway_Nativity_Directory.MaskedTextBox-   'Conway_Nativity_Directory.MaskedTextBox*   PreviewMouseDown+ �� 	Converter  borderStyle)   #Conway_Nativity_Directory.TagEditor)   #Conway_Nativity_Directory.TagEditor,   PreviewMouseDown��%F  '
        7   &	 q    '��     '  �    &	     &	 :    �� 5�  *   " 
 60   .�5�  .   ;� 6S   * MainWindow_PreviewMouseDown�� 5�  .    �@��
IsReadOnly 6K   �� *
IsSelected+8+    �� $ ListViewItemL��� 5�  .    � ��
 ���� 6K   �� 5�  .    � �� 6L   Z� �� 5�  .    � ��$ 0q�6P   �� 5�  .    � �� 6K   �� $ 
Foreground� �� $ ListViewItemL� 5�  .   ^� 5�  2    5�  6   �� 5�  :    6D   �� $ 
IsSelected� True6h   �� 5�  :    6D   �� $ IsKeyboardFocused� �� *Self+ True5�  P   0�5�  6   �� 5�  :    � �� �	#504689FF6W   5�  3   5�  +   �� 5�  *   " ��60   .�5�  .   ��  � ��$ 1q�6P   �� 5�  .    �@J�SnapsToDevicePixels . 6T    5�  .   ^� 5�  2    5�  6   �� 5�  :    6D   �� $ 
IsSelected� True6h   �� 5�  :    6D   �� $ IsMouseOver� �� *Self+ True5�  P   0�5�  6   �� 5�  :    � �� �	#FF4689FF6X   �� 5�  :    � �� �	#304689FF6W   5�  3   ^� 5�  2    5�  6   �� 5�  :    6D   �� $ 
IsSelected� True6h   �� 5�  :    6D   �� $ IsKeyboardFocused� �� *Self+ True5�  P   0�5�  6   �� 5�  :    � �� �	#FF4689FF6X   5�  3   5�  +   �� 5�  *   " ��60    5�  .   P� 5�  2    �@J�IsMouseOver . . �5�  6   ��  � �� �	#19008BFF6S   �� 5�  6    � �� �	#19008BFF6T   5�  3   5�  +   �� 5�  *   "  60   .�5�  .   ;� 6S   , MainWindow_PreviewMouseDown5�  +   �� 5�  *   .@�5�  .   m� -p   5        $��Remove��5�  7   6G   m� 5�  .   $��Image Tools��67   .@�5�  2   m� $'��!addImageSingleNativityContextMenu��6;   -q   5        $��Add Image...��5�  d   5�  7   m� 5�  2   $*��$removeImageSingleNativityContextMenu��6;   -r   5        $��Remove Image...��5�  g   5�  7   �� 5�  2   m� 5�  2   $��Rotation��6;   .@�5�  6   m� -s   5        $��Rotate Right��5�  ?   $ Ctrl+Shift+R��6w   m� 5�  6   -t   5        $��Rotate Left��5�  ?   $ Ctrl+Shift+L��6u   5�  3   m� 5�  2   -u   5        $��Embedd Image��5�  ;   6Q   m� 5�  2   -v   5        $��Relink Image...��5�  ;   6T   5�  /   m� 5�  .   $��Tagging��67   .@�5�  2   m� -w   5        $*��$Organize Tags & Geographical Origins��5�  ;   $ Ctrl+Shift+O��5�  d   m� 5�  2   -x   5        $��Add Tag��5�  ;   $ Ctrl+Shift+T��5�  Y   m� 5�  2   -y   5        $��
Remove Tag��5�  ;   $ 
Ctrl+Alt+T��5�  ^   m� 5�  2   -z   5        $��Add Geographical Origin��5�  ;   $ Ctrl+Shift+G��5�  h   m� 5�  2   -{   5        $ ��Remove Geographical Origin��5�  ;   $ 
Ctrl+Alt+G��5�  m   5�  /   m� 5�  .   $��Naming��67   .@�5�  2   m� -|   5        $��Uppercase...��5�  ;   6Q   m� 5�  2   -}   5        $��Lowercase...��5�  ;   6Q   m� 5�  2   -~   5        $��Fix Capitalization...��5�  ;   6Z   m� 5�  2   -   5        $��Replace Text...��5�  ;   6T   m� 5�  2   -�   5        $��Replace Text (Regex)...��5�  ;   6\   5�  /   5�  +   �� 5�  *   .@�5�  .   m� -�   5        $��Remove��5�  7   6G   m� 5�  .   $��Image Tools��67   .@�5�  2   m� -�   5        $��Remove Images...��5�  ;   6U   �� 5   2   m� 5  2   $��Rotation��6;   .@�5  6   m� -�   5        $��Rotate Right��5  ?   $ Ctrl+Shift+R��6w   m� 5  6   -�   5        $��Rotate Left��5  ?   $ Ctrl+Shift+L��6u   5  3   m� 5  2   -�   5        $��Embedd Images��5  ;   6R   m� 5  2   -�   5        $��Relink Images...��5  ;   6U   5  /   m� 5  .   $��Tagging��67   .@�5	  2   m� -�   5        $*��$Organize Tags & Geographical Origins��5	  ;   $ Ctrl+Shift+O��5
  d   m� 5  2   -�   5        $��Add Tag to Selection...��5  ;   $ Ctrl+Shift+T��5  Y   m� 5  2   -�   5        $"��Remove Tag from Selection...��5  ;   $ 
Ctrl+Alt+T��5  ^   m� 5  2   -�   5        $-��'Add Geographical Origin to Selection...��5  ;   $ Ctrl+Shift+G��5  h   m� 5  2   -�   5        $2��,Remove Geographical Origin from Selection...��5  ;   $ 
Ctrl+Alt+G��5  m   5  /   m� 5  .   $��Naming��67   .@�5  2   m� -�   5        $��Uppercase...��5  ;   6Q   m� 5  2   -�   5        $��Lowercase...��5  ;   6Q   m� 5  2   -�   5        $��Fix Capitalization...��5  ;   6Z   m� 5  2   -�   5        $��Replace Text...��5  ;   6T   m� 5  2   -�   5        $��Replace Text (Regex)...��5  ;   6\   5  /   5  +   5  '   - �� View- 5  &   �� 5  *   .P�5   .   �� -�   5        idGridViewColumn��. �� Width$
. Auto��5   W   / �� CellTemplate/ 5!  2   �� 5"  6   .R�5#  :   �� #��[ 6A   ._�5$  >   
  0 
 Name$0 	idTextBox��6R   1   Loaded1 columnTextBox_Loaded5'  >   2   UserStoppedTyping%2 !nativityTextBox_UserStoppedTyping5(  >   3 
  	RegexMask$3 [^0-9]+��5$  ~   $��0��5&  >   ��5$  e   �� $ Id� 5%  >   �� 5(  >   5)  ;   5*  7   .��5,  2   �� -�   5        4 ��Name$4 IdColumn��5,  G   
����������	#FFF3F3F3��5.  6   �� 5/  :   .+�50  >   1   +Conway_Nativity_Directory.ColumnHeaderArrow  -�   5        5  Name$5 idArrow��50  V   $��8��6g   $��4��6q   �� 51  >   $��Id��6H   $��Center=�6R   52  ;    54  6   ��%�   '        �� 55  :   "  6@   .�56  >   ��  � �� 57  1   �� $ 
Foreground� �� $ GridViewColumnHeaderL�59  ;   5:  7   5;  3   5<  /   �� 5>  .   -�   5        titleGridViewColumn��$
. Auto��5>  Z   / 5?  2   �� 5@  6   .R�5A  :   �� #��[ 6A   ._�5B  >   
  $0 titleTextBox��6R   1 columnTextBox_Loaded5E  >   %2 !nativityTextBox_UserStoppedTyping5F  >   $��1��5D  >   ��5B  h   �� $ Title� 5C  >   �� 5F  >   5G  ;   5H  7   .��5J  2   �� -�   5        $4 titleColumn��5J  G   
����������	#FFF3F3F3��5L  6   �� 5M  :   .+�5N  >     -�   5        $5 
titleArrow��5N  V   $��8��6j   $��4��6t   �� 5O  >   $��Title��6H   $��Center=�6U   5P  ;    5R  6   ��%�   '        �� 5S  :   "  6@   .�5T  >   ��  � �� 5U  1   �� $ 
Foreground� �� $ GridViewColumnHeaderL�5W  ;   5X  7   5Y  3   5Z  /   �� 5\  .   -�   5        originGridViewColumn��$
. Auto��5\  [   / 5]  2   �� 5^  6   .R�5_  :   �� #��[ 6A   ._�5`  >   
  $0 originTextBox��6R   1 columnTextBox_Loaded5c  >   %2 !nativityTextBox_UserStoppedTyping5d  >   $��2��5b  >   ��5`  i   �� $ Origin� 5a  >   �� 5d  >   5e  ;   5f  7   .��5h  2   �� -�   5        $4 originColumn��5h  G   
����������	#FFF3F3F3��5j  6   �� 5k  :   .+�5l  >     -�   5        $5 originArrow��5l  V   $��8��6k   $��4��6u   �� 5m  >   $��Origin��6H   $��Center=�6V   5n  ;    5p  6   ��%�   '        �� 5q  :   "  6@   .�5r  >   ��  � �� 5s  1   �� $ 
Foreground� �� $ GridViewColumnHeaderL�5u  ;   5v  7   5w  3   5x  /   �� 5z  .   -�   5        acquiredGridViewColumn��$
. Auto��5z  ]   / 5{  2   �� 5|  6   .R�5}  :   �� #��[ 6A   ._�5~  >   
  $0 acquiredTextBox��6R   1 columnTextBox_Loaded5�  >   %2 !nativityTextBox_UserStoppedTyping5�  >   $3 [^0-9]+��5~  �   $��3��5�  >   ��5~  k   �� $ Acquired� 5  >   �� 5�  >   5�  ;   5�  7   .��5�  2   �� -�   5        $4 acquiredColumn��5�  G   
����������	#FFF3F3F3��5�  6   �� 5�  :   .+�5�  >     -�   5        $5 acquiredArrow��5�  V   $��8��6m   $��4��6w   �� 5�  >   $��Acquired��6H   $��Center=�6X   5�  ;    5�  6   ��%�   '        �� 5�  :   "  6@   .�5�  >   ��  � �� 5�  1   �� $ 
Foreground� �� $ GridViewColumnHeaderL�5�  ;   5�  7   5�  3   5�  /   �� 5�  .   -�   5        fromGridViewColumn��$
. Auto��5�  Y   / 5�  2   �� 5�  6   .R�5�  :   �� #��[ 6A   ._�5�  >   
  $0 fromTextBox��6R   1 columnTextBox_Loaded5�  >   %2 !nativityTextBox_UserStoppedTyping5�  >   $��4��5�  >   ��5�  g   �� $
 From� 5�  >   �� 5�  >   5�  ;   5�  7   .��5�  2   �� -�   5        $4 
fromColumn��5�  G   
����������	#FFF3F3F3��5�  6   �� 5�  :   .+�5�  >     -�   5        $5 	fromArrow��5�  V   $��8��6i   $��4��6s   �� 5�  >   $
��From��6H   $��Center=�6T   5�  ;    5�  6   ��%�   '        �� 5�  :   "  6@   .�5�  >   ��  � �� 5�  1   �� $ 
Foreground� �� $ GridViewColumnHeaderL�5�  ;   5�  7   5�  3   5�  /   �� 5�  .   -�   5        costGridViewColumn��$
. Auto��5�  Y   / 5�  2   �� 5�  6   .R�5�  :   �� $! 
Horizontal=�6E   .+�5�  >   �� $��Center=�6H   $��$��6c   �� 5�  >   #��[ 6E   ._�5�  B   
  $0 costTextBox��6V   1 columnTextBox_Loaded5�  >   %2 !nativityTextBox_UserStoppedTyping5�  >   6 
  NumericOnly6 . $��5��5�  >   ��5�  k   �� $
 Cost� 5�  >   �� 5�  >   5�  ?   5�  ;   5�  7   .��5�  2   �� -�   5        $4 
costColumn��5�  G   
����������	#FFF3F3F3��5�  6   �� 5�  :   .+�5�  >     -�   5        $5 	costArrow��5�  V   $��8��6i   $��4��6s   �� 5�  >   $
��Cost��6H   $��Center=�6T   5�  ;    5�  6   ��%�   '        �� 5�  :   "  6@   .�5�  >   ��  � �� 5�  1   �� $ 
Foreground� �� $ GridViewColumnHeaderL�5�  ;   5�  7   5�  3   5�  /   �� 5�  .   -�   5        locationGridViewColumn��$
. Auto��5�  ]   / 5�  2   �� 5�  6   .R�5�  :   �� #��[ 6A   ._�5�  >   
  $0 locationTextBox��6R   1 columnTextBox_Loaded5�  >   %2 !nativityTextBox_UserStoppedTyping5�  >   $��6��5�  >   ��5�  k   �� $ Location� 5�  >   �� 5�  >   5�  ;   5�  7   .��5�  2   �� -�   5        $4 locationColumn��5�  G   
����������	#FFF3F3F3��5�  6   �� 5�  :   .+�5�  >     -�   5        $5 locationArrow��5�  V   $��8��6m   $��4��6w   �� 5�  >   $��Location��6H   $��Center=�6X   5�  ;    5�  6   ��%�   '        �� 5�  :   "  6@   .�5�  >   ��  � �� 5�  1   �� $ 
Foreground� �� $ GridViewColumnHeaderL�5�  ;   5�  7   5�  3   5�  /   �� 5�  .   -�   5        tagsGridViewColumn��$
. Auto��5�  Y   / 5�  2   �� 5�  6   .R�5�  :     7  Name$7 tagsTagEditor��5�  6   8   UserChangedTags8 T_TagEditor_UserChangedTags5�  6   9   Loaded9 columnTextBox_Loaded5�  >   $��16��5�  J   :   
TagsSource: 6V   �� $
 Tags� 5�  6   �� 5�  6   5�  7   .��5�  2   �� -�   5        $4 
tagsColumn��5�  G   
����������	#FFF3F3F3��5  6   �� 5  :   .+�5  >     -�   5        $5 	tagsArrow��5  V   $��8��6i   $��4��6s   �� 5  >   $
��Tags��6H   $��Center=�6T   5  ;    5  6   ��%�   '        �� 5  :   "  6@   .�5	  >   ��  � �� 5
  1   �� $ 
Foreground� �� $ GridViewColumnHeaderL�5  ;   5  7   5  3   5  /   �� 5  .   -�   5        %!geographicalOriginsGridViewColumn��$
. Auto��5  h   / 5  2   �� 5  6   .R�5  :     $"7 geographicalOriginsTagEditor��5  6    8 GO_TagEditor_UserChangedTags5  6   9 columnTextBox_Loaded5  >   $��16��5  J   : 6V   �� $ GeographicalOrigins� 5  6   �� 5  6   5  7   .��5  2   �� -�   5        $4 geographicalOriginsColumn��5  G   
����������	#FFF3F3F3��5  6   �� 5  :   .+�5  >     -�   5        $5 geographicalOriginsArrow��5  V   $��8��6x   $��4��6�   �� 5   >   $��Geographical Origins��6H   $��Center=�6d   5!  ;    5#  6   ��%�   '        �� 5$  :   "  6@   .�5%  >   ��  � �� 5&  1   �� $ 
Foreground� �� $ GridViewColumnHeaderL�5(  ;   5)  7   5*  3   5+  /   5,  +   5.  #   �� 5/  "   $��0��6-   $��2��6=   $��	10,5,5,10q�6J   $! 
Horizontal=�6]   .+�50  &   �� -�   5        $��nativityCountTextBlock��50  0   $��0 Nativities��6N   �� 51  &   $! 
Horizontal=�61   .+�52  *   �� $��, ��64   �� 53  *   ��64   �� $ SelectedItems.Count�$# nativityListView���� 54  *   $��	 Selected��64   ��55  *   �� 56  .   " ��64    57  2   �� 58  6    6B   �� $ SelectedItems.Count�$# nativityListView��  06�   .a�59  :   ��  � y�$ 	Collapsed=�6W   5:  7   5<  /   5>  '   5?  #   5@     5A     5B     5C     � 5D     -�   5        $ infoPanesGrid��5D     $��2��6.   $��0��6>   $��3��6K   $��5q�6\   $��Stretch=�6g   $��Stretch=�6�   .+�5E       -�   5        $ mainInfoPane��5E  !   $
 main��67   .@�5F       $��Nativity Information��6$   $ nativity��6B   .��5G     �� -�   5        $��informationContent��5G  )   $
��Auto=�6C   $
��Auto=�6u   5H     5I     5J     5K     5L     5M     5N     5O     