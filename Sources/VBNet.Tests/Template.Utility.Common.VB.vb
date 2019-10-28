'Imports System.IO
'Imports System.Text
'Imports EnvDTE
'Imports System.Reflection

''T4_TEMPLATE_HEADER_BEGIN~
''<#@ assembly name="System.Core"
''#><#@ assembly name="System.Data.Linq"
''#><#@ assembly name="EnvDTE"
''#><#@ assembly name="System.Xml"
''#><#@ assembly name="System.Xml.Linq"
''#><#@ import namespace="System"
''#><#@ import namespace="System.CodeDom"
''#><#@ import namespace="System.CodeDom.Compiler"
''#><#@ import namespace="System.Collections.Generic"
''#><#@ import namespace="System.Data.Linq"
''#><#@ import namespace="System.Data.Linq.Mapping"
''#><#@ import namespace="System.IO"
''#><#@ import namespace="EnvDTE"
''#><#@ import namespace="System.Linq"
''#><#@ import namespace="System.Reflection"
''#><#@ import namespace="System.Text"
''#><#@ import namespace="System.Text.RegularExpressions"
''#><#@ import namespace="System.Xml.Linq"
''#><#@ import namespace="System.Globalization"
''#><#@ import namespace="Microsoft.VisualStudio.TextTemplating"
''~T4_TEMPLATE_HEADER_END~

'Public Class FileManager
'    Private ReadOnly files As New List(Of Block)()
'    Private ReadOnly footer As New Block()
'    Private ReadOnly header As New Block()
'    Protected _textTransformation As DynamicTextTransformation
'    Private m_currentBlock As Block
'    Protected generatedFileNames As New List(Of [String])()
'    Protected m_outputPath As String
'    Private templateDirectory As String
'    Protected m_templateFile As String

'    Private Sub New(textTransformation As DynamicTextTransformation)
'        ', string templateDirectory)
'        Me._textTransformation = textTransformation
'        Me.m_templateFile = textTransformation.Host.TemplateFile
'        ' Path.GetDirectoryName(templatingHost.EngineHost.TemplateFile);
'        Me.templateDirectory = Path.GetDirectoryName(m_templateFile)
'    End Sub

'    Public ReadOnly Property TemplateFile() As String
'        Get
'            Return m_templateFile
'        End Get
'    End Property

'    Public Property OutputPath() As [String]
'        Get
'            Return m_outputPath
'        End Get
'        Set(value As [String])
'            If value <> m_outputPath Then
'                m_outputPath = value
'                If m_outputPath(m_outputPath.Length - 1) <> Path.DirectorySeparatorChar Then
'                    m_outputPath += Path.DirectorySeparatorChar
'                    'OutputPathChanged();
'                End If
'            End If
'        End Set
'    End Property

'    Protected Overridable Function ResolveOutputPath() As String
'        Dim result As String = Me.OutputPath

'        If Not String.IsNullOrEmpty(Me.OutputPath) Then
'            If Not Path.IsPathRooted(Me.OutputPath) Then
'                result = Path.Combine(templateDirectory, Me.OutputPath)
'            End If
'        Else
'            result = templateDirectory
'        End If

'        Return result
'    End Function

'    Public Overridable ReadOnly Property DefaultProjectNamespace() As [String]
'        Get
'            Return Nothing
'        End Get
'    End Property

'    Private Property CurrentBlock() As Block
'        Get
'            Return m_currentBlock
'        End Get
'        Set(value As Block)
'            If CurrentBlock IsNot Nothing Then
'                EndBlock()
'            End If
'            If value IsNot Nothing Then
'                value.Start = Me._textTransformation.GenerationEnvironment.Length
'            End If
'            m_currentBlock = value
'        End Set
'    End Property

'    Public Shared Function Create(textTransformation As Object) As FileManager
'        Dim result As FileManager = Nothing
'        Dim inVSNET As Boolean = False

'        Dim transformation As DynamicTextTransformation = DynamicTextTransformation.Create(textTransformation)
'        Dim host As IDynamicHost = transformation.Host

'        Try

'#If Not PREPROCESSED_TEMPLATE Then
'            If host.AsIServiceProvider() IsNot Nothing Then
'                inVSNET = True
'            End If
'#End If

'            result = If(inVSNET, New VSManager(transformation), New FileManager(transformation))
'            'host.LogError(e);
'        Catch e As Exception
'        End Try
'        Return result
'    End Function

'    'protected virtual void OutputPathChanged()
'    '    {
'    '    }


'    Public Sub StartNewFile(name As [String])
'        If name Is Nothing Then
'            Throw New ArgumentNullException("name")
'        End If
'        CurrentBlock = New Block() With {.Name = name}
'    End Sub

'    Public Sub EndNewFile()
'        EndBlock()
'    End Sub

'    Public Sub StartFooter()
'        CurrentBlock = footer
'    End Sub

'    Public Sub EndFooter()
'        EndBlock()
'    End Sub

'    Public Sub StartHeader()
'        CurrentBlock = header
'    End Sub

'    Public Sub EndHeader()
'        EndBlock()
'    End Sub

'    Public Sub Flush()
'        Me.Process()
'    End Sub

'    Private Sub EndBlock()
'        If CurrentBlock Is Nothing Then
'            Return
'        End If

'        CurrentBlock.Length = Me.Template.Length - CurrentBlock.Start
'        If CurrentBlock IsNot header AndAlso CurrentBlock IsNot footer Then
'            files.Add(CurrentBlock)
'        End If
'        m_currentBlock = Nothing
'    End Sub

'    Protected ReadOnly Property Template() As StringBuilder
'        Get
'            Return Me._textTransformation.GenerationEnvironment
'        End Get
'    End Property

'    Protected Friend Overridable Sub Process()
'        'bool split)
'        'Debugger.Break();
'        'if (split)
'        If True Then
'            EndBlock()
'            Dim headerText As [String] = Me.Template.ToString(header.Start, header.Length)
'            Dim footerText As [String] = Me.Template.ToString(footer.Start, footer.Length)
'            Dim outputPath As [String] = ResolveOutputPath()
'            'string.IsNullOrEmpty(OutputPath) ? Path.GetDirectoryName(host.TemplateFile) : OutputPath;
'            files.Reverse()
'            For Each block As Block In files
'                Dim fileName As [String] = Path.Combine(outputPath, block.Name)
'                Dim content As [String] = headerText + Me.Template.ToString(block.Start, block.Length) + footerText
'                generatedFileNames.Add(fileName)
'                CreateFile(fileName, content)
'                Me.Template.Remove(block.Start, block.Length)
'            Next
'        End If
'    End Sub

'    Protected Overridable Sub CreateFile(fileName As [String], content As [String])
'        If IsFileContentDifferent(fileName, content) Then
'            File.WriteAllText(fileName, content)
'        End If
'    End Sub

'    Public Overridable Function GetCustomToolNamespace(fileName As [String]) As [String]
'        Return Nothing
'    End Function

'    Public Overridable Function GetTemplateItemCustomToolNamespace() As String
'        Return String.Empty
'    End Function

'    Public Function ResolveNamespace() As String
'        If Not String.IsNullOrEmpty(Me.DefaultProjectNamespace) Then
'            Return Me.DefaultProjectNamespace
'        End If

'        Return GetTemplateItemCustomToolNamespace()
'    End Function

'    Protected Function IsFileContentDifferent(fileName As [String], newContent As [String]) As Boolean
'        Return Not (File.Exists(fileName) AndAlso File.ReadAllText(fileName) = newContent)
'    End Function

'#Region "Nested type: Block"

'    Private Class Block
'        Public Length As Integer
'        Public Name As [String]
'        Public Start As Integer
'    End Class

'#End Region

'#Region "Nested type: VSManager"

'    Private Class VSManager
'        Inherits FileManager
'        Private ReadOnly checkOutAction As Action(Of [String])
'        Private ReadOnly dte As DTE
'        Private ReadOnly projectSyncAction As Action(Of IEnumerable(Of [String]))
'        Private ReadOnly templateProjectItem As ProjectItem
'        Private outputPathProjectItem As ProjectItem
'        Private oldProjectItemsToDelete As List(Of ProjectItem)

'        Friend Sub New(templatingHost As DynamicTextTransformation)
'            MyBase.New(templatingHost)
'            Dim hostServiceProvider = _textTransformation.Host.AsIServiceProvider()
'            If hostServiceProvider Is Nothing Then
'                Throw New ArgumentNullException("Could not obtain hostServiceProvider")
'            End If

'            dte = DirectCast(hostServiceProvider.GetService(GetType(EnvDTE.DTE)), EnvDTE.DTE)
'            If dte Is Nothing Then
'                Throw New ArgumentNullException("Could not obtain DTE from host")
'            End If
'            templateProjectItem = dte.Solution.FindProjectItem(m_templateFile)

'            BuildOldTemplateProjectFilesToDelete()

'            checkOutAction = Function(fileName As [String]) dte.SourceControl.CheckOutItem(fileName)
'            projectSyncAction = Function(keepFileNames)
'                                    ProjectSync(outputPathProjectItem, oldProjectItemsToDelete, keepFileNames)
'                                End Function

'            'Function(keepFileNames As IEnumerable(Of [String])) ProjectSync(outputPathProjectItem, oldProjectItemsToDelete, keepFileNames)
'        End Sub

'        Private Sub BuildOldTemplateProjectFilesToDelete()
'            oldProjectItemsToDelete = templateProjectItem.ProjectItems.OfType(Of ProjectItem)().ToList()
'        End Sub

'        Public Overrides ReadOnly Property DefaultProjectNamespace() As [String]
'            Get
'                Return templateProjectItem.ContainingProject.Properties.Item("DefaultNamespace").Value.ToString()
'            End Get
'        End Property

'        Public Overrides Function GetCustomToolNamespace(fileName As String) As [String]
'            Return dte.Solution.FindProjectItem(fileName).Properties.Item("CustomToolNamespace").Value.ToString()
'        End Function

'        Public Overridable Overloads Function GetTemplateItemCustomToolNamespace() As String
'            Return If(templateProjectItem IsNot Nothing, templateProjectItem.Properties.Item("CustomToolNamespace").Value.ToString(), String.Empty)
'        End Function

'        Protected Friend Overrides Sub Process()
'            'bool split)
'            If templateProjectItem.ProjectItems Is Nothing Then
'                Return
'            End If
'            MyBase.Process()
'            projectSyncAction.EndInvoke(projectSyncAction.BeginInvoke(generatedFileNames, Nothing, Nothing))
'        End Sub

'        Protected Overrides Sub CreateFile(fileName As [String], content As [String])
'            If IsFileContentDifferent(fileName, content) Then
'                CheckoutFileIfRequired(fileName)
'                File.WriteAllText(fileName, content)
'            End If
'        End Sub

'        'protected override void OutputPathChanged()
'        '        {
'        '            outputPathProjectItem = FindProjectItem(dte,
'        '                templateProjectItem.ContainingProject.ProjectItems, ResolveOutputPath());
'        '                //templateProjectItem.ContainingProject.ProjectItems, OutputPath);
'        '            Debug.WriteLine("outputPathProjectItem: " + (outputPathProjectItem == null ? "null" : "not null"));
'        '        }


'        Protected Overrides Function ResolveOutputPath() As String
'            Dim result As String = MyBase.ResolveOutputPath()
'            outputPathProjectItem = FindProjectItem(dte, templateProjectItem.ContainingProject.ProjectItems, result)

'            If Not String.IsNullOrEmpty(result) Then
'                Dim dir As String = Path.GetDirectoryName(result)
'                If Directory.Exists(dir) Then
'                    Dim templateFileName As String = templateProjectItem.FileNames(0)
'                    Dim templateDir As String = Path.GetDirectoryName(templateFileName)

'                    If String.Compare(dir, templateDir, True) = 0 Then
'                        outputPathProjectItem = templateProjectItem
'                    End If
'                End If
'            End If

'            If outputPathProjectItem Is Nothing Then
'                outputPathProjectItem = templateProjectItem
'            End If

'            Return result
'        End Function

'        Private Shared Function FindProjectItem(vsObj As DTE, inProjectItems As ProjectItems, ProjectItemName As String) As ProjectItem
'            For Each projItem As ProjectItem In inProjectItems
'                For i As Short = 0 To projItem.FileCount - 1
'                    If projItem.FileNames(i) = ProjectItemName Then
'                        Return projItem
'                    ElseIf projItem.ProjectItems.Count > 0 Then
'                        Dim found As ProjectItem = FindProjectItem(vsObj, projItem.ProjectItems, ProjectItemName)
'                        If found IsNot Nothing Then
'                            Return found
'                        End If
'                    End If
'                Next
'            Next

'            Return Nothing
'        End Function

'        Private Shared Sub ProjectSync(templateProjectItem As ProjectItem, projectItemsToDelete As List(Of ProjectItem), keepFileNames As IEnumerable(Of [String]))
'            Dim keepFileNameSet = New HashSet(Of [String])(keepFileNames)
'            Dim projectFiles = New Dictionary(Of [String], ProjectItem)()
'            Dim templateFileName As String = templateProjectItem.FileNames(0)
'            For Each projectItem As ProjectItem In templateProjectItem.ProjectItems
'                projectFiles.Add(projectItem.FileNames(0), projectItem)
'            Next

'            If projectItemsToDelete IsNot Nothing Then
'                For Each oldProjectItem As ProjectItem In projectItemsToDelete
'                    Dim oldFileName As String = oldProjectItem.FileNames(0)
'                    If oldFileName <> templateFileName AndAlso Not keepFileNames.Contains(oldFileName) Then
'                        oldProjectItem.Delete()
'                    End If
'                Next
'            End If


'            ' Add missing files to the project

'            For Each fileName As [String] In keepFileNameSet
'                If Not projectFiles.ContainsKey(fileName) Then
'                    templateProjectItem.ProjectItems.AddFromFile(fileName)
'                End If
'            Next
'        End Sub

'        Private Sub CheckoutFileIfRequired(fileName As [String])
'            Dim sc As SourceControl = dte.SourceControl
'            If sc IsNot Nothing AndAlso sc.IsItemUnderSCC(fileName) AndAlso Not sc.IsItemCheckedOut(fileName) Then
'                checkOutAction.EndInvoke(checkOutAction.BeginInvoke(fileName, Nothing, Nothing))
'            End If
'        End Sub
'    End Class

'#End Region
'End Class

' ''' <summary>
' ''' Reponsible for abstracting the use of Host between times
' ''' when it is available and not
' ''' </summary>
'Public Interface IDynamicHost
'    ''' <summary>
'    ''' An abstracted call to Microsoft.VisualStudio.TextTemplating.ITextTemplatingEngineHost ResolveParameterValue
'    ''' </summary>
'    Function ResolveParameterValue(id As String, name As String, otherName As String) As String

'    ''' <summary>
'    ''' An abstracted call to Microsoft.VisualStudio.TextTemplating.ITextTemplatingEngineHost ResolvePath
'    ''' </summary>
'    Function ResolvePath(path As String) As String

'    ''' <summary>
'    ''' An abstracted call to Microsoft.VisualStudio.TextTemplating.ITextTemplatingEngineHost TemplateFile
'    ''' </summary>
'    ReadOnly Property TemplateFile() As String

'    ''' <summary>
'    ''' Returns the Host instance cast as an IServiceProvider
'    ''' </summary>
'    Function AsIServiceProvider() As IServiceProvider
'End Interface

' ''' <summary>
' ''' Reponsible for implementing the IDynamicHost as a dynamic
' ''' shape wrapper over the Microsoft.VisualStudio.TextTemplating.ITextTemplatingEngineHost interface
' ''' rather than type dependent wrapper.  We don't use the
' ''' interface type so that the code can be run in preprocessed mode
' ''' on a .net framework only installed machine.
' ''' </summary>
'Public Class DynamicHost
'    Implements IDynamicHost
'    Private ReadOnly _instance As Object
'    Private ReadOnly _resolveParameterValue As MethodInfo
'    Private ReadOnly _resolvePath As MethodInfo
'    Private ReadOnly _templateFile As PropertyInfo

'    ''' <summary>
'    ''' Creates an instance of the DynamicHost class around the passed in
'    ''' Microsoft.VisualStudio.TextTemplating.ITextTemplatingEngineHost shapped instance passed in.
'    ''' </summary>
'    Public Sub New(ByVal instance As Object)
'        _instance = instance
'        Dim type As Type = _instance.[GetType]()
'        _resolveParameterValue = type.GetMethod("ResolveParameterValue", New Type() {GetType(String), GetType(String), GetType(String)})
'        _resolvePath = type.GetMethod("ResolvePath", New Type() {GetType(String)})

'        _templateFile = type.GetProperty("TemplateFile")
'    End Sub

'    ''' <summary>
'    ''' A call to Microsoft.VisualStudio.TextTemplating.ITextTemplatingEngineHost ResolveParameterValue
'    ''' </summary>
'    Public Function ResolveParameterValue(ByVal id As String, ByVal name As String, ByVal otherName As String) As String Implements IDynamicHost.ResolveParameterValue
'        Return DirectCast(_resolveParameterValue.Invoke(_instance, New Object() {id, name, otherName}), String)
'    End Function

'    ''' <summary>
'    ''' A call to Microsoft.VisualStudio.TextTemplating.ITextTemplatingEngineHost ResolvePath
'    ''' </summary>
'    Public Function ResolvePath(ByVal path As String) As String Implements IDynamicHost.ResolvePath
'        Return DirectCast(_resolvePath.Invoke(_instance, New Object() {path}), String)
'    End Function

'    ''' <summary>
'    ''' A call to Microsoft.VisualStudio.TextTemplating.ITextTemplatingEngineHost TemplateFile
'    ''' </summary>
'    Public ReadOnly Property TemplateFile() As String Implements IDynamicHost.TemplateFile
'        Get
'            Return DirectCast(_templateFile.GetValue(_instance, Nothing), String)
'        End Get
'    End Property

'    ''' <summary>
'    ''' Returns the Host instance cast as an IServiceProvider
'    ''' </summary>
'    Public Function AsIServiceProvider() As IServiceProvider Implements IDynamicHost.AsIServiceProvider
'        Return TryCast(_instance, IServiceProvider)
'    End Function
'End Class

' ''' <summary>
' ''' Reponsible for implementing the IDynamicHost when the
' ''' Host property is not available on the TextTemplating type. The Host
' ''' property only exists when the hostspecific attribute of the template
' ''' directive is set to true.
' ''' </summary>
'Public Class NullHost
'    Implements IDynamicHost
'    ''' <summary>
'    ''' An abstraction of the call to Microsoft.VisualStudio.TextTemplating.ITextTemplatingEngineHost ResolveParameterValue
'    ''' that simply retuns null.
'    ''' </summary>
'    Public Function ResolveParameterValue(id As String, name As String, otherName As String) As String Implements IDynamicHost.ResolveParameterValue
'        Return Nothing
'    End Function

'    ''' <summary>
'    ''' An abstraction of the call to Microsoft.VisualStudio.TextTemplating.ITextTemplatingEngineHost ResolvePath
'    ''' that simply retuns the path passed in.
'    ''' </summary>
'    Public Function ResolvePath(path As String) As String Implements IDynamicHost.ResolvePath
'        Return path
'    End Function

'    ''' <summary>
'    ''' An abstraction of the call to Microsoft.VisualStudio.TextTemplating.ITextTemplatingEngineHost TemplateFile
'    ''' that returns null.
'    ''' </summary>
'    Public ReadOnly Property TemplateFile() As String Implements IDynamicHost.TemplateFile
'        Get
'            Return Nothing
'        End Get
'    End Property

'    ''' <summary>
'    ''' Returns null.
'    ''' </summary>
'    Public Function AsIServiceProvider() As IServiceProvider Implements IDynamicHost.AsIServiceProvider
'        Return Nothing
'    End Function
'End Class

' ''' <summary>
' ''' Responsible creating an instance that can be passed
' ''' to helper classes that need to access the TextTransformation
' ''' members.  It accesses member by name and signature rather than
' ''' by type.  This is necessary when the
' ''' template is being used in Preprocessed mode
' ''' and there is no common known type that can be
' ''' passed instead
' ''' </summary>
'Public Class DynamicTextTransformation
'    Private _instance As Object
'    Private _dynamicHost As IDynamicHost

'    Private ReadOnly _write As MethodInfo
'    Private ReadOnly _writeLine As MethodInfo
'    Private ReadOnly _generationEnvironment As PropertyInfo
'    Private ReadOnly _errors As PropertyInfo
'    Private ReadOnly _host As PropertyInfo

'    ''' <summary>
'    ''' Creates an instance of the DynamicTextTransformation class around the passed in
'    ''' TextTransformation shapped instance passed in, or if the passed in instance
'    ''' already is a DynamicTextTransformation, it casts it and sends it back.
'    ''' </summary>
'    Public Shared Function Create(instance As Object) As DynamicTextTransformation
'        If instance Is Nothing Then
'            Throw New ArgumentNullException("instance")
'        End If

'        Dim textTransformation As DynamicTextTransformation = TryCast(instance, DynamicTextTransformation)
'        If textTransformation IsNot Nothing Then
'            Return textTransformation
'        End If

'        Return New DynamicTextTransformation(instance)
'    End Function

'    Private Sub New(instance As Object)
'        _instance = instance
'        Dim type As Type = _instance.[GetType]()
'        _write = type.GetMethod("Write", New Type() {GetType(String)})
'        _writeLine = type.GetMethod("WriteLine", New Type() {GetType(String)})
'        _generationEnvironment = type.GetProperty("GenerationEnvironment", BindingFlags.Instance Or BindingFlags.NonPublic)
'        _host = type.GetProperty("Host")
'        _errors = type.GetProperty("Errors")
'    End Sub

'    ''' <summary>
'    ''' Gets the value of the wrapped TextTranformation instance's GenerationEnvironment property
'    ''' </summary>
'    Public ReadOnly Property GenerationEnvironment() As StringBuilder
'        Get
'            Return DirectCast(_generationEnvironment.GetValue(_instance, Nothing), StringBuilder)
'        End Get
'    End Property

'    ''' <summary>
'    ''' Gets the value of the wrapped TextTranformation instance's Errors property
'    ''' </summary>
'    Public ReadOnly Property Errors() As System.CodeDom.Compiler.CompilerErrorCollection
'        Get
'            Return DirectCast(_errors.GetValue(_instance, Nothing), System.CodeDom.Compiler.CompilerErrorCollection)
'        End Get
'    End Property

'    ''' <summary>
'    ''' Calls the wrapped TextTranformation instance's Write method.
'    ''' </summary>
'    Public Sub Write(text As String)
'        _write.Invoke(_instance, New Object() {text})
'    End Sub

'    ''' <summary>
'    ''' Calls the wrapped TextTranformation instance's WriteLine method.
'    ''' </summary>
'    Public Sub WriteLine(text As String)
'        _writeLine.Invoke(_instance, New Object() {text})
'    End Sub

'    ''' <summary>
'    ''' Gets the value of the wrapped TextTranformation instance's Host property
'    ''' if available (shows up when hostspecific is set to true in the template directive) and returns
'    ''' the appropriate implementation of IDynamicHost
'    ''' </summary>
'    Public ReadOnly Property Host() As IDynamicHost
'        Get
'            If _dynamicHost Is Nothing Then
'                If _host Is Nothing Then
'                    _dynamicHost = New NullHost()
'                Else
'                    _dynamicHost = New DynamicHost(_host.GetValue(_instance, Nothing))
'                End If
'            End If
'            Return _dynamicHost
'        End Get
'    End Property
'End Class