   M S B A M L   `   `   `  ����N  JMultiSelectTreeView, Version=1.0.9.0, Culture=neutral, PublicKeyToken=nullA%clr-namespace:System.Windows.ControlsSystem.Windows.Controls  5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0894   .System.Windows.Controls.ThicknessLeftConverter   ThicknessLeftConverter5   /System.Windows.Controls.MultiSelectTreeViewItem  �� 
TargetType �� Triggers �� Binding �� RelativeSource �� AncestorType1   +System.Windows.Controls.MultiSelectTreeView �� FallbackValue �� Value �� Property �� Value	 K� IsItemsHost
 �� 	Resources  Key �� Color �� 
StartPoint �� EndPoint � Color � Offset)   #System.Windows.Controls.EditTextBox �� Mode �� UpdateSourceTrigger-  'System.Windows.Input.KeyboardNavigation �� Path �� Mode �� AncestorLevel �� 
TargetType � ColumnDefinitions �� 	ClickMode d� 	IsChecked �� Padding �� Angle �� CenterX �� CenterY �� Triggers P� Property P� Value  �� 
TargetName! ]� 
Conditions" �� Property# �� Value$ �� Orientation% J� SnapsToDevicePixels& �� CornerRadius' +� IsFocusScope( �� HeaderStringFormat) �� ContentStringFormat* J� IsHitTestVisible+   Remarks,   RemarksTemplate- �� 	Converter. �� Stretch/ �� StrokeDashArray0 �� 
SourceName  HoveredBackgroundBrush  HoveredBorderBrush  SelectedBackgroundBrush  SelectedBorderBrush  FocusedBorderBrush " SelectedHoveredBackgroundBrush1 ^� 
Conditions2 �� Binding  InactiveBorderBrush 	 InactiveBackgroundBrush�� 5      D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    :x,http://schemas.microsoft.com/winfx/2006/xaml     5      2Controls%clr-namespace:System.Windows.Controls  5      %Z   &	        '       7   7  7  7  7  7  7  7  7 	 7  7 	 7  7  7  7     5      6#   �� 5      "   6	    5	      �� 5
       5      �� *)'(VirtualizingStackPanel.IsVirtualizing)+ �� "  $
 True�� True5      .a�5      ��  � �� 5      �� 5      .R�5   	   $� 	 . 6    5   	   5      5      
 5      ���� 5      HoveredBorderBrush $ 	#b097c3f9��60   �� 5      HoveredBackgroundBrush $	 0,0&�68   $	 0,1&�6I   .��5      � $ #fafbfd��6   $ 0Q�6#   � 5      $ #ebf3fd��6   $ 1Q�6#   5      �� 5      SelectedBorderBrush $ #7da2ce��61   �� 5       SelectedBackgroundBrush $	 0,0&�69   $	 0,1&�6J   .��5!      � $ #ebf4fe��6   $ 0Q�6#   � 5"      $ #cfe4fe��6   $ 1Q�6#   5#      �� 5%      "SelectedHoveredBackgroundBrush $	 0,0&�6@   $	 0,1&�6Q   .��5&      � $ #dcebfc��6   $ 0Q�6#   � 5'      $ #c1dbfc��6   $ 1Q�6#   5(      �� 5*      FocusedBorderBrush $ #7da2ce��60   �� 5,      InactiveBorderBrush $ 	#b5c9c9c9��61   �� 5-      InactiveBackgroundBrush $	 0,0&�69   $	 0,1&�6J   .��5.      � $ #f8f8f8��6   $ 0Q�6#   � 5/      $ #d8d8d8��6   $ 1Q�6#   50      51      .�53      ��  �@ RemarksTemplate 54      �� 55      .R�56      �� $��Center=�6   $��3,0,0,0q�6,   ��6=   �� ��57      �� 58   	   "  ��6    59   
   �� 5:       6   ��  6+   Z� .a�5;      ��  � y�$ 	Collapsed=�6)   5<      �� 5=       6   ��   6+   .a�5>      ��  � y�$ 	Collapsed=�6)   5?      5A   
   5C      5D      5F      �� 5G       �@ ContentTemplateEdit 5H      �� 5I      .R�5J        $��0q�6k   $��1q�6   ��6   �� *DisplayName+$ TwoWay=�$ PropertyChanged=�6   5K      5M      �� 5O       �@ 	IsTabStop .  63   �� 5P       �@%�VirtualizationMode 5Q      �� *-+(VirtualizingStackPanel.VirtualizationMode)+ �� "  $ Standard���� 5R       �@%�IsVirtualizing .  6<   �� 5S       � �� �	#00FFFFFF6'   �� 5T       � �� 5U      �� $  HorizontalContentAlignment� �� $ FindAncestor=�$ ItemsControlL�$ 1��$
 Left���� 5V       � �� 5W      �� $ VerticalContentAlignment� �� $ FindAncestor=�$ ItemsControlL�$ 1��$ Center���� 5X       � ��$ 1q�6&   �� 5Y       � ��
 ���� 6"   �� 5Z       � ��$ 1q�6&   �� 5[       � ��$ 0q�6   �� 5]       �@�FocusVisualStyle 68   Z� �� 5`       �@ HoverHighlighting 5a      �� *20(Controls:MultiSelectTreeView.HoverHighlighting)+ �� "  $ OneWay=�$ False���� 5b       �@ 
ItemIndent 5c      �� *+)(Controls:MultiSelectTreeView.ItemIndent)+ �� "  $ OneWay=�$ 13���� 5d       �@ IsKeyboardMode 5e      �� */-(Controls:MultiSelectTreeView.IsKeyboardMode)+ �� "  $ OneWay=�$ False���� 5g       � �� 5h      �� 5i      "  6   .R�5k      �� .+�5l      �  5m   	   �� 5n   
   $
��Auto �6   �� 5p   
   $
��Auto �6   .+�5t   	   d� $��Expander��5v      $ Press=�6	    5u   	   ��  �� "  $ 
IsExpanded�$ False����5w   
   �� 5x      "  d�6   
 5y      �� 5z      .�5|      ��  � |� .  62   �� 5}       � ��$ 12��65   �� 5~       � ��$ 16��66   �� 5       � �� 5�      �� 5�      " d�6   .R�5�      �� $	 3,5q�6   ���	#00FFFFFF$��12��6;   $��16��6F   ._�5�      F� $��
ExpandPath��69   ���	#00FFFFFF���	#FF989898��5�      B� 5�      $��M0,0 L0,6 L6,0 zC�6   z�5�      �� 5�      $	 135Q�6"   $ 3Q�6.   $ 3Q�6:   5�      5�       5�      P� 5�       �@J�IsMouseOver . . �5�      �� $  
ExpandPath��60    � �� 5�      ��5�      	#FF1BBBFA��6.   5�      �� 5�      $  
ExpandPath��6.    � �� 5�      ��5�      	#00FFFFFF��6.   5�      5�      P� 5�       �@d�	IsChecked$
 TrueY�6:   . �5�      �� $  
ExpandPath��6=    � z� 5�      �� 5�      $	 180Q�6#   $ 3Q�6/   $ 3Q�6;   5�      �� 5�      $  
ExpandPath��6.    � �� 5�      ��5�      	#FF595959��6.   5�      �� 5�      $  
ExpandPath��60    � �� 5�      ��5�      	#FF262626��6.   5�      5�      ]� 5�      ! 5�      �� 5�      " �@J�IsMouseOver# . 6=   �� 5�      " �@d�	IsChecked$
# TrueY�6>   /�5�      �� 5�      $  
ExpandPath��61    � �� 5�      ��5�      	#FF1BBBFA��6/   5�      �� 5�      $  
ExpandPath��6/    � �� 5�      ��5�      	#701BBBFA��6/   5�      5�      5�      5�      5�      5�   
   �� 5�   	   $��1��6   $$ 
Horizontal=�6$   .+�5�   
   � $��ElementGrid��6   .+�5�      �� $��headerBorder��6   #��z��5�      # z��5�      $��0,0,0,-1q�5�      #��z��5�      #��z��5�      % . $& 2��5�   &   $��Center=�5�      ._�5�      �� $��PART_Header��6   ' . #��z��5�      #��z��5�      #) z( 5�      #��z��5�      $��Header��5�      #��z��5�      #��z��5�      #% z% 5�      5�      �� 5�      $��1q�6   ���	#80ffffff$��1,1,1,0q�6>   % . $& 2��6j   * .  6{   5�      �� 5�   
   #��z+ 5�   
   #��z, 5�   
   #% z% 5�   
   5�   
   5�   	   � 5�      $��ItemsHostGrid��6   ��6"   �� *
ItemIndent+8-    5�   	   �� 5�   
   $
��Auto �6   �� 5�   
   $��* �6   .+�5�   	   F� $��	RulerPath��61   ����        �    @B 	$. Uniform=�6   ���	#60000000$/ 0.5 2.5R�5�      % . $��0,0,1,0q�5�   $   �� 5�   	   $��	ItemsHost��6(   $��1��6   6(   5�   	   5�       5�      P� 5�       �@ 	IsVisible .  . �5�   	   ��  � y�$ 	Collapsed=�67   5�   	   ]� 5     ! 5  	   �� 5  
   " �@ HoverHighlighting# . 6R   �� 5  
   $0 ElementGrid��6Y   " �@J�IsMouseOver# . ./�5  	   �� 8   � ��6.   �� 5  	   8   � ��6.   5  	   P� 5      �@ 
IsSelected . . �5  	   �� 8   � ��6.   �� 5  	   8   � ��6.   5  	   P� 5      �@J�	IsFocused . . �5  	   �� 8   � ��6.   5  	   ]� 5     ! 5  	   �� 5  
   " �@J�	IsFocused# . 6J   �� 5  
   " �@ 
IsSelected# . 6K   ./�5  	   �� 8   � ��6.   5  	   ^� 5     1 5   	   �� 5!  
   2 5"  
   ��  �� " �$ IsActive�$ False��	# False5#  
   �� 5$  
   2 5%  
   ��  �� $
 Self=�$ 
IsSelected�# True5&  
   .0�5(  	   �� 8   � ��6.   �� 5)  	   8   � ��6.   5*  	   ^� 5-     1 5.  	   �� 5/  
   2 50  
   ��  �� "  $ IsKeyboardFocusWithin�$ False��	# False51  
   �� 52  
   2 53  
   ��  �� "  $ ContextMenu.IsOpen�$ False��	# False54  
   �� 55  
   2 56  
   ��  �� $
 Self=�$ 
IsSelected�# True57  
   .0�59  	   �� 8 	  � ��6.   �� 5:  	   8 
  � ��6.   5;  	   ]� 5?     ! 5@  	   �� 5A  
   " �@ HoverHighlighting# . 6R   �� 5B  
   $0 ElementGrid��6Y   " �@J�IsMouseOver# . �� 5C  
   " �@ 
IsSelected# . 6*   ./�5E  	   �� 8   � ��6.   �� 5F  	   8   � ��6.   5G  	   P� 5J      �@ 
IsExpanded .  . �5K  	   �� $  ItemsHostGrid��60    � y�$ 	Collapsed=�6K   5L  	   P� 5O      �@��HasItems .  . �5P  	   �� $  Expander��60    � y�$ Hidden=�6F   5Q  	   P� 5T      � {� .  . �5U  	   �� # � ��62    � ��5V  	   �� 5Y     $ False��5[      5Z     ��  �� "  $ VerticalRulers�$ False��.a�5\  	   �� $  	RulerPath��6    � y�$ 	Collapsed=�6=   5]  	   P� 5`      �@ 	IsEditing . . �5a  	   ��  5b  	   �� *ContentTemplateEdit+ �� *Self+ � ���� 5c  	   8   � ��6.   �� 5d  	   8   � ��6.   5e  	   5g     5i     5j     5k     