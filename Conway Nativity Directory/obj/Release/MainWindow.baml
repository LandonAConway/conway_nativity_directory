   M S B A M L   `   `   `  ���� T  PConway Nativity Directory, Version=1.2.1.0, Culture=neutral, PublicKeyToken=nullE'clr-namespace:Conway_Nativity_DirectoryConway_Nativity_Directory  > :Api, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullb<clr-namespace:ConwayNativityDirectory.PluginApi;assembly=Api!ConwayNativityDirectory.PluginApi *    $Conway_Nativity_Directory.MainWindow   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     9local'clr-namespace:Conway_Nativity_Directory     Mpapi<clr-namespace:ConwayNativityDirectory.PluginApi;assembly=Api       � Title$  Conway Nativity Directory��$	��450��$	��800�� � WindowState$ 	Maximized=� � 	Resources 7   1Conway_Nativity_Directory.BooleanInverseConverter   booleanInverseConverter<   6Conway_Nativity_Directory.BooleanToVisibilityConverter   booleanToVisibilityConverter@   :Conway_Nativity_Directory.SortingTypeToVisibilityConverter   SortingType  idVisibility  titleVisibility  originVisibility  acquiredVisibility  fromVisibility  costVisibility  locationVisibility>   8Conway_Nativity_Directory.AllowEditingTimeStampConverter "	 allowEditingTimestampConverter 
 	ClearPath��%  &	        &	      &	 
     &	      &	 )     &	 ;     &	 O     &	 _     &	 o     &		 �     &	
 �           $ Id=�  $ Title=�  $ Origin=�  $ Acquired=�  $
 From=�  $
 Cost=�  $ Location=�  B�M0,0 L1,1 M1,0 L0,1� J� CommandBindings )  #System.Windows.Input.CommandBinding  -      Command$ local:CustomCommands.New��  -   $ local:CustomCommands.Open��  -   $  local:CustomCommands.Close��  -   $ local:CustomCommands.Save��  -   $! local:CustomCommands.SaveAs��  -   $  local:CustomCommands.Login��  -   $! local:CustomCommands.Logout��  -   $ local:CustomCommands.Undo��  -	   $ local:CustomCommands.Redo��  -
   $+ %local:CustomCommands.RotateImageRight��  -   $* $local:CustomCommands.RotateImageLeft��  -   $) #local:CustomCommands.AdvancedSearch��  -   $ local:CustomCommands.Find��  -   $ local:CustomCommands.GoTo��  -   $ local:CustomCommands.Add��  -   $! local:CustomCommands.Remove��  -   $$ local:CustomCommands.SelectAll��  -   $% local:CustomCommands.SelectNone��  -   $' !local:CustomCommands.SelectInvert��  -   $/ )local:CustomCommands.CreateTagForSelected��  -   $0 *local:CustomCommands.RemoveTagFromSelected��  -   $> 8local:CustomCommands.CreateGeographicalOriginForSelected��  -   $? 9local:CustomCommands.RemoveGeographicalOriginFromSelected��  -   $= 7local:CustomCommands.OrganizeTagsAndGeographicalOrigins��  -   $# local:CustomCommands.Commands��  -   $ local:CustomCommands.Exit�� J� InputBindings �� 
 �� Key$ N�� �� 	Modifiers$
 Ctrlf�	 �� Command$	 local:CustomCommands.New���� $ O��$
 Ctrlf�$	 local:CustomCommands.Open���� $ W��$
 Ctrlf�$ 	 local:CustomCommands.Close���� $ S��$
 Ctrlf�$	 local:CustomCommands.Save���� $ S��$ 
Ctrl+Shiftf�$!	 local:CustomCommands.SaveAs���� $ L��$
 Ctrlf�$ 	 local:CustomCommands.Login���� $ L��$	 Altf�$!	 local:CustomCommands.Logout���� $ Z��$
 Ctrlf�$	 local:CustomCommands.Undo���� $ Y��$
 Ctrlf�$	 local:CustomCommands.Redo���� $ R��$ 
Ctrl+Shiftf�$+	 %local:CustomCommands.RotateImageRight���� $ L��$ 
Ctrl+Shiftf�$*	 $local:CustomCommands.RotateImageLeft���� $ S��$ 	Shift+Altf�$)	 #local:CustomCommands.AdvancedSearch���� $ F��$
 Ctrlf�$	 local:CustomCommands.Find���� $ G��$ 	Shift+Altf�$	 local:CustomCommands.GoTo���� $ A��$ 	Shift+Altf�$	 local:CustomCommands.Add���� $ R��$ 	Shift+Altf�$!	 local:CustomCommands.Remove���� $ Delete��$
 Ctrlf�$!	 local:CustomCommands.Remove���� $ A��$ 
Ctrl+Shiftf�$$	 local:CustomCommands.SelectAll���� $ N��$ 
Ctrl+Shiftf�$%	 local:CustomCommands.SelectNone���� $ I��$ 
Ctrl+Shiftf�$'	 !local:CustomCommands.SelectInvert���� $ T��$ 
Ctrl+Shiftf�$/	 )local:CustomCommands.CreateTagForSelected���� $ T��$ Ctrl+Altf�$0	 *local:CustomCommands.RemoveTagFromSelected���� $ G��$ 
Ctrl+Shiftf�$>	 8local:CustomCommands.CreateGeographicalOriginForSelected���� $ G��$ Ctrl+Altf�$?	 9local:CustomCommands.RemoveGeographicalOriginFromSelected���� $ O��$ 
Ctrl+Shiftf�$=	 7local:CustomCommands.OrganizeTagsAndGeographicalOrigins���� $ F2��$ Shiftf�$#	 local:CustomCommands.Commands���� $ F1��$	 Altf�$	 local:CustomCommands.Exit��.��� 
 � RowDefinitions
 �� $
��Auto ��� �� $
��Auto �.+�o� $��24��$��0��  �� 
TargetType �� Triggers ^� 
Conditions �� Binding �� Path �� RelativeSource �� AncestorType*   $Conway_Nativity_Directory.MainWindow �� Value �� Property �� Value  collapsableSep��%L  'm�       &	 �     �� " m� ^�  ��  �� $  LoginSettings.AllowEditing� �� "  	 False��  �� $	 Tag� �� *Self+ Collapsable0���  � y�$ 	Collapsed=��� " �� ^�  ��  �� $  LoginSettings.AllowEditing� �� "  	 False0���  � y�$ 	Collapsed=�.@�m� -   $��fileMenuItem��$
��File��.@�m� -   $��newMenuItem��$	��New��
 � Tag$ Collapsable�� m� InputGestureText$ Ctrl+N��m� -   $��openMenuItem��$��Open...��$ Collapsable��$ Ctrl+O���� #���  m� -   $��closeMenuItem��$��Close��$ Collapsable��$ Ctrl+W��m� -   $��saveMenuItem��$
��Save��$ Collapsable��$ Ctrl+S��m� -    $��saveAsMenuItem��$��
Save As...��$ Collapsable��$ Ctrl+Shift+S���� #���  m� -!   $��importMenuItem��$ Collapsable��$��Import��.@�m� -"   $��parseNarrativesMenuItem��$��
Narratives��m� -#   $��exportMenuItem��$ Collapsable��$��Export��.@�m� -$   $��exportNarrativesMenuItem��$��
Narratives���� #���  m� -%   $��loginMenuItem��$��Login...��$ Ctrl+L��m� -&   $��logoutMenuItem��$��Logout��$ Alt+L������ " m��� m� -'   $��exitMenuItem��$
��Exit��$ Alt+F1��m� -(   $��projectMenuItem��$��Project��$ Collapsable��.@�m� -)   $��settingsMenuItem��$��Settings...��m� -*   $��editMenuItem��$
��Edit��$ Collapsable��.@�m� -+   $��undoMenuItem��$
��Undo��$ Ctrl+Z��m� -,   $��redoMenuItem��$
��Redo��$ Ctrl+R���� m� --   $��imageMenuItem��$��Image Tools��.@�m� -.   $��assignImagesByIdMenuItem��$��Assign Images by Id...��m� -/   $��removeImagesMenuItem��$��Remove Images...���� m� -0   $��rotationMenuItem��$��Rotation��.@�m� -1   $��rotateRightMenuItem��$��Rotate Right��$ Ctrl+Shift+R��m� -2   $��rotateLeftMenuItem��$��Rotate Left��$ Ctrl+Shift+L��m� -3   $��embeddImagesMenuItem��$��Embedd Images��m� -4   $��relinkImagesMenuItem��$��Relink Images...��m� -5   $��taggingMenuItem��$��Tagging��.@�m� -6   $(��"organizeTagsAndGeographicalOrigins��$*��$Organize Tags & Geographical Origins��$ Ctrl+Shift+O��m� -7   $��addTagToSelectionMenuItem��$��Add Tag to Selection...��$ Ctrl+Shift+T��m� -8   $$��removeTagFromSelectionMenuItem��$"��Remove Tag from Selection...��$ 
Ctrl+Alt+T��m� -9   $.��(addGeographicalOriginToSelectionMenuItem��$-��'Add Geographical Origin to Selection...��$ Ctrl+Shift+G��m� -:   $3��-removeGeographicalOriginFromSelectionMenuItem��$2��,Remove Geographical Origin from Selection...��$ 
Ctrl+Alt+G��m� -;   $��namingMenuItem��$��Naming��.@�m� -<   $��uppercaseMenuItem��$��Uppercase...��m� -=   $��lowercaseMenuItem��$��Lowercase...��m� ->   $��fixCapitalizationMenuItem��$��Fix Capitalization...���� m� -?   $��preferencesMenuItem��$��Preferences��m� -@   $��nativitiesMenuItem��$��
Nativities��$ Collapsable��.@�m� -A   $��addNativityMenuItem��$��Add...��$ Shift+Alt+A��m� -B   $��removeNativityMenuItem��$��Remove��$ Shift+Alt+R���� m� $��	Selection��.@�m� $��Select��.@�m� -C   $��selectAllMenuItem��$	��All��$ Ctrl+Shift+A��m� -D   $��selectNoneMenuItem��$
��None��$ Ctrl+Shift+N��m� -E   $��selectInvertMenuItem��$��Invert��$ Ctrl+Shift+I��m� -F   $��searchMenuItem��$��Search��.@�m� -G   $��findMenuItem��$��Find...��$ Ctrl+F��m� -H   $��goToMenuItem��$��Go To...��$ Shift+Alt+G��m� -I   $��advancedSearchMenuItem��$��Advanced Search...��$ Shift+Alt+S���� #���  m� -J   $��searchToolsMenuItem��$��Tools��$ Collapsable��.@�m� -K   $$��findMissingNativityIdsMenuItem��$"��Find Missing Nativity Ids...��m� -L   $!��findNativitiesWithoutImages��$'��!Find Nativities Without Images...���� $y�	Collapsed=�m� -M   $��highlightToolMenuItem��$��Highlight Tool...��$y�	Collapsed=�m� -N   $��viewMenuItem��$
��View��$ Collapsable��.@�m� -O   $��workspacesMenuItem��$��
Workspaces���� m� -P   $��viewIdColumnMenuItem��$��Id�� m� IsCheckable . m� -Q   $��viewTitleColumnMenuItem��$��Title�� . m� -R   $��viewOriginColumnMenuItem��$��Origin�� . m� -S   $ ��viewAcquiredColumnMenuItem��$��Acquired�� . m� -T   $��viewFromColumnMenuItem��$
��From�� . m� -U   $��viewCostColumnMenuItem��$
��Cost�� . m� -V   $ ��viewLocationColumnMenuItem��$��Location�� . m� -W   $��viewTagsColumnMenuItem��$
��Tags�� . m� -X   $+��%viewGeographicalOriginsColumnMenuItem��$��Geographical Origins�� . �� m� -Y   $��pluginsMenuItem��$��Plugins��$ Collapsable��.@�m� -Z   $��managePluginsMenuItem��$��	Manage...��m� -[   $��helpMenuItem��$
��Help��.@�m� -\   $��Changelogs...���� m� -]   $��About...��� -^   $��projectGrid��$��1��
 �� $
��Auto ��� �� $
��Auto � � ColumnDefinitions �� �� $
��Auto ��� $
��0.5* �.+��� $��1��$��0��$��1��$��3��$��2.5,0q�$��Stretch=�� -_    �Name$ viewPanesGrid��$��0��$��3��$��5q�.+�$   Conway_Nativity_Directory.Pane  -`     Name$ mainViewPane��   WorkspaceKey$
 main��.@�0  *ConwayNativityDirectory.PluginApi.PaneItem  $��
Nativities��
   Key$ nativity��.��� 
 �� $
��Auto ��� �� $
��Auto �.+�� $��0��$��0��$��22��$��	10,10,5,5q�.+��� -a   $��searchTextBox���� -b   ._��� $��Advanced Search��$��Center=�$	��5,0q����� " ��.���  � ��# Z���  � y�$ 	Collapsed=� P�  P� Property �@J�IsMouseOver P� Value . . ���  � �� �	#FF98CCF5��  �� Binding �� $ AdvancedSearchIsActive� �� "    �� Value  True.a���  � y�$ Visible=��� $��1q�! �� Orientation$! 
Horizontal=�$��RightToLeft=�.+��� -c   $��advancedSearchButton��
������ ���� $ ActualHeight� �� *Self+��Z� ��F� $��1��
�������" �� Stretch$" Uniform=�O����        ���-     �    @B ���- @B �    �� ���- �� �    ��- ���- ��- 	$��4q����� " ��.���  � y�$ 	Collapsed=� ��  �� $
 Text�# �� ElementName$# searchTextBox��   .a���  � y�$ Visible=���  �� $ AdvancedSearchIsActive� �� "    True.a���  � y�$ 	Collapsed=��� -d   $��clearSeachButton��
������ ���� $ ActualHeight� �� *Self+��Z� ��F� $��1��
�������$" Uniform=�#��� 
 $��4q����� " �� ��  �� $
 Text�$# searchTextBox��   .a���  � y�$ 	Collapsed=���  �� $ AdvancedSearchIsActive� �� "    True.a���  � y�$ Visible=��� -e   $��nativityListView��$��1��$��0��$��10,5,5,5q�$ %� IsVirtualizing$ . ���� .R�>	   8Conway_Nativity_Directory.NativityVirtualizingStackPanel	  ���� " �� P�  �@��
IsSelected . . ���  � �� �	#4C008BFF��  � �� �	#FF4689FF��  � �� �	#7F008BFFP�  �@J�IsMouseOver . . ���  � �� �	#19008BFF��  � �� �	#4C008BFF.���  � �� �� % �� 
TargetType"% ��.R��� & �� Padding#& z��#��z��#��z��#��z��' J� SnapsToDevicePixels' . ._��� #��z��#��z��#' z' ��  �@��
IsSelected �� *
IsSelected+( �� Mode$( TwoWay=�) �� UpdateSourceTrigger$) PropertyChanged=� -
   'Conway_Nativity_Directory.CustomTextbox-   'Conway_Nativity_Directory.CustomTextbox*   PreviewMouseDown+ �� 	Converter  borderStyle)   #Conway_Nativity_Directory.TagEditor)   #Conway_Nativity_Directory.TagEditor,   PreviewMouseDown��%�  '
        7   &	 	    '�� �    '  R    �� " 
 .�;� * MainWindow_PreviewMouseDown��  �@��
IsReadOnly �� *
IsSelected+8+    �� $ ListViewItemL���  � ��
 ���� ��  � �� Z� ��  � ��$ 0q���  � �� �� $ 
Foreground� �� $ ListViewItemL��� " ��.���  � ��$ 1q���  �@J�SnapsToDevicePixels .  ^�  ��  �� $ 
IsSelected� True��  �� $ IsMouseOver� �� *Self+ True0���  � �� �	#FF4689FF�� " �� P�  �@J�IsMouseOver . . ���  � �� �	#19008BFF��  � �� �	#19008BFF�� "  .�;� , MainWindow_PreviewMouseDown- �� View- �� .P��� -f   idGridViewColumn��. �� Width$
. Auto��/ �� CellTemplate/ �� .R��� #��[ ._�
  0 
 Name$0 	idTextBox��1   Loaded1 columnTextBox_Loaded2   UserStoppedTyping%2 !nativityTextBox_UserStoppedTyping3 
  	RegexMask$3 [^0-9]+��$��0������ $ Id� �� .���� -g   4 ��Name$4 IdColumn��
����������	#FFF3F3F3���� .+�1   +Conway_Nativity_Directory.ColumnHeaderArrow  -h   5  Name$5 idArrow��$��8��$��4���� $��Id��$��Center=� ��%b   '        �� "  .���  � �� �� $ 
Foreground� �� $ GridViewColumnHeaderL��� -i   titleGridViewColumn��$
. Auto��/ �� .R��� #��[ ._�
  $0 titleTextBox��1 columnTextBox_Loaded%2 !nativityTextBox_UserStoppedTyping$��1������ $ Title� �� .���� -j   $4 titleColumn��
����������	#FFF3F3F3���� .+�  -k   $5 
titleArrow��$��8��$��4���� $��Title��$��Center=� ��%b   '        �� "  .���  � �� �� $ 
Foreground� �� $ GridViewColumnHeaderL��� -l   originGridViewColumn��$
. Auto��/ �� .R��� #��[ ._�
  $0 originTextBox��1 columnTextBox_Loaded%2 !nativityTextBox_UserStoppedTyping$��2������ $ Origin� �� .���� -m   $4 originColumn��
����������	#FFF3F3F3���� .+�  -n   $5 originArrow��$��8��$��4���� $��Origin��$��Center=� ��%b   '        �� "  .���  � �� �� $ 
Foreground� �� $ GridViewColumnHeaderL��� -o   acquiredGridViewColumn��$
. Auto��/ �� .R��� #��[ ._�
  $0 acquiredTextBox��1 columnTextBox_Loaded%2 !nativityTextBox_UserStoppedTyping$3 [^0-9]+��$��3������ $ Acquired� �� .���� -p   $4 acquiredColumn��
����������	#FFF3F3F3���� .+�  -q   $5 acquiredArrow��$��8��$��4���� $��Acquired��$��Center=� ��%b   '        �� "  .���  � �� �� $ 
Foreground� �� $ GridViewColumnHeaderL��� -r   fromGridViewColumn��$
. Auto��/ �� .R��� #��[ ._�
  $0 fromTextBox��1 columnTextBox_Loaded%2 !nativityTextBox_UserStoppedTyping$��4������ $
 From� �� .���� -s   $4 
fromColumn��
����������	#FFF3F3F3���� .+�  -t   $5 	fromArrow��$��8��$��4���� $
��From��$��Center=� ��%b   '        �� "  .���  � �� �� $ 
Foreground� �� $ GridViewColumnHeaderL��� -u   costGridViewColumn��$
. Auto��/ �� .R��� $! 
Horizontal=�.+��� $��Center=�$��$���� #��[ ._�
  $0 costTextBox��1 columnTextBox_Loaded%2 !nativityTextBox_UserStoppedTyping6 
  NumericOnly6 . $��5������ $
 Cost� �� .���� -v   $4 
costColumn��
����������	#FFF3F3F3���� .+�  -w   $5 	costArrow��$��8��$��4���� $
��Cost��$��Center=� ��%b   '        �� "  .���  � �� �� $ 
Foreground� �� $ GridViewColumnHeaderL��� -x   locationGridViewColumn��$
. Auto��/ �� .R��� #��[ ._�
  $0 locationTextBox��1 columnTextBox_Loaded%2 !nativityTextBox_UserStoppedTyping$��6������ $ Location� �� .���� -y   $4 locationColumn��
����������	#FFF3F3F3���� .+�  -z   $5 locationArrow��$��8��$��4���� $��Location��$��Center=� ��%b   '        �� "  .���  � �� �� $ 
Foreground� �� $ GridViewColumnHeaderL��� -{   tagsGridViewColumn��$
. Auto��/ �� .R�  7  Name$7 tagsTagEditor��8   UserChangedTags8 T_TagEditor_UserChangedTags9   Loaded9 columnTextBox_Loaded$��16��:   
TagsSource: �� $
 Tags� �� .���� -|   $4 
tagsColumn��
����������	#FFF3F3F3���� .+�  -}   $5 	tagsArrow��$��8��$��4���� $
��Tags��$��Center=� ��%b   '        �� "  .���  � �� �� $ 
Foreground� �� $ GridViewColumnHeaderL��� -~   %!geographicalOriginsGridViewColumn��$
. Auto��/ �� .R�  $"7 geographicalOriginsTagEditor�� 8 GO_TagEditor_UserChangedTags9 columnTextBox_Loaded$��16��: �� $ GeographicalOrigins� �� .���� -   $4 geographicalOriginsColumn��
����������	#FFF3F3F3���� .+�  -�   $5 geographicalOriginsArrow��$��8��$��4���� $��Geographical Origins��$��Center=� ��%b   '        �� "  .���  � �� �� $ 
Foreground� �� $ GridViewColumnHeaderL��� $��0��$��2��$��	10,5,5,10q�$! 
Horizontal=�.+��� -�   $��nativityCountTextBlock��$��0 Nativities���� $! 
Horizontal=�.+��� $��, ���� ���� $ SelectedItems.Count�$# nativityListView���� $��	 Selected������ " �� ��  �� $ SelectedItems.Count�$# nativityListView��  0.a���  � y�$ 	Collapsed=�� -�   $ infoPanesGrid��$��2��$��0��$��3��$��5q�$��Stretch=�$��Stretch=�.+�  -�   $ mainInfoPane��$
 main��.@�  $��Nativity Information��$ nativity��.���� -�   $��informationContent��$
��Auto=�$
��Auto=�