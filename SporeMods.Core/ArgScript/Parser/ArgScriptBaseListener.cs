//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     ANTLR Version: 4.9.1
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from Z:/Linux/projects/Spore-Mod-Manager/SporeMods.Core/ArgScript\ArgScript.g4 by ANTLR 4.9.1

// Unreachable code detected
#pragma warning disable 0162
// The variable '...' is assigned but its value is never used
#pragma warning disable 0219
// Missing XML comment for publicly visible type or member '...'
#pragma warning disable 1591
// Ambiguous reference in cref attribute
#pragma warning disable 419


using Antlr4.Runtime.Misc;
using IErrorNode = Antlr4.Runtime.Tree.IErrorNode;
using ITerminalNode = Antlr4.Runtime.Tree.ITerminalNode;
using IToken = Antlr4.Runtime.IToken;
using ParserRuleContext = Antlr4.Runtime.ParserRuleContext;

/// <summary>
/// This class provides an empty implementation of <see cref="IArgScriptListener"/>,
/// which can be extended to create a listener which only needs to handle a subset
/// of the available methods.
/// </summary>
[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.9.1")]
[System.Diagnostics.DebuggerNonUserCode]
[System.CLSCompliant(false)]
public partial class ArgScriptBaseListener : IArgScriptListener {
	/// <summary>
	/// Enter a parse tree produced by <see cref="ArgScriptParser.root"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterRoot([NotNull] ArgScriptParser.RootContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="ArgScriptParser.root"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitRoot([NotNull] ArgScriptParser.RootContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="ArgScriptParser.statement"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterStatement([NotNull] ArgScriptParser.StatementContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="ArgScriptParser.statement"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitStatement([NotNull] ArgScriptParser.StatementContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="ArgScriptParser.include"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterInclude([NotNull] ArgScriptParser.IncludeContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="ArgScriptParser.include"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitInclude([NotNull] ArgScriptParser.IncludeContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="ArgScriptParser.pragma"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterPragma([NotNull] ArgScriptParser.PragmaContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="ArgScriptParser.pragma"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitPragma([NotNull] ArgScriptParser.PragmaContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="ArgScriptParser.block"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterBlock([NotNull] ArgScriptParser.BlockContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="ArgScriptParser.block"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitBlock([NotNull] ArgScriptParser.BlockContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="ArgScriptParser.if"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterIf([NotNull] ArgScriptParser.IfContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="ArgScriptParser.if"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitIf([NotNull] ArgScriptParser.IfContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="ArgScriptParser.elif"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterElif([NotNull] ArgScriptParser.ElifContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="ArgScriptParser.elif"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitElif([NotNull] ArgScriptParser.ElifContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="ArgScriptParser.else"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterElse([NotNull] ArgScriptParser.ElseContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="ArgScriptParser.else"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitElse([NotNull] ArgScriptParser.ElseContext context) { }
	/// <summary>
	/// Enter a parse tree produced by the <c>op</c>
	/// labeled alternative in <see cref="ArgScriptParser.expression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterOp([NotNull] ArgScriptParser.OpContext context) { }
	/// <summary>
	/// Exit a parse tree produced by the <c>op</c>
	/// labeled alternative in <see cref="ArgScriptParser.expression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitOp([NotNull] ArgScriptParser.OpContext context) { }
	/// <summary>
	/// Enter a parse tree produced by the <c>comp</c>
	/// labeled alternative in <see cref="ArgScriptParser.expression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterComp([NotNull] ArgScriptParser.CompContext context) { }
	/// <summary>
	/// Exit a parse tree produced by the <c>comp</c>
	/// labeled alternative in <see cref="ArgScriptParser.expression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitComp([NotNull] ArgScriptParser.CompContext context) { }
	/// <summary>
	/// Enter a parse tree produced by the <c>paren</c>
	/// labeled alternative in <see cref="ArgScriptParser.expression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterParen([NotNull] ArgScriptParser.ParenContext context) { }
	/// <summary>
	/// Exit a parse tree produced by the <c>paren</c>
	/// labeled alternative in <see cref="ArgScriptParser.expression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitParen([NotNull] ArgScriptParser.ParenContext context) { }
	/// <summary>
	/// Enter a parse tree produced by the <c>bool</c>
	/// labeled alternative in <see cref="ArgScriptParser.expression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterBool([NotNull] ArgScriptParser.BoolContext context) { }
	/// <summary>
	/// Exit a parse tree produced by the <c>bool</c>
	/// labeled alternative in <see cref="ArgScriptParser.expression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitBool([NotNull] ArgScriptParser.BoolContext context) { }
	/// <summary>
	/// Enter a parse tree produced by the <c>atomic</c>
	/// labeled alternative in <see cref="ArgScriptParser.expression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterAtomic([NotNull] ArgScriptParser.AtomicContext context) { }
	/// <summary>
	/// Exit a parse tree produced by the <c>atomic</c>
	/// labeled alternative in <see cref="ArgScriptParser.expression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitAtomic([NotNull] ArgScriptParser.AtomicContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="ArgScriptParser.call"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterCall([NotNull] ArgScriptParser.CallContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="ArgScriptParser.call"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitCall([NotNull] ArgScriptParser.CallContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="ArgScriptParser.param"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterParam([NotNull] ArgScriptParser.ParamContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="ArgScriptParser.param"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitParam([NotNull] ArgScriptParser.ParamContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="ArgScriptParser.ref"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterRef([NotNull] ArgScriptParser.RefContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="ArgScriptParser.ref"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitRef([NotNull] ArgScriptParser.RefContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="ArgScriptParser.command"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterCommand([NotNull] ArgScriptParser.CommandContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="ArgScriptParser.command"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitCommand([NotNull] ArgScriptParser.CommandContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="ArgScriptParser.argument"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterArgument([NotNull] ArgScriptParser.ArgumentContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="ArgScriptParser.argument"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitArgument([NotNull] ArgScriptParser.ArgumentContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="ArgScriptParser.literal"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterLiteral([NotNull] ArgScriptParser.LiteralContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="ArgScriptParser.literal"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitLiteral([NotNull] ArgScriptParser.LiteralContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="ArgScriptParser.keyword"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterKeyword([NotNull] ArgScriptParser.KeywordContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="ArgScriptParser.keyword"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitKeyword([NotNull] ArgScriptParser.KeywordContext context) { }

	/// <inheritdoc/>
	/// <remarks>The default implementation does nothing.</remarks>
	public virtual void EnterEveryRule([NotNull] ParserRuleContext context) { }
	/// <inheritdoc/>
	/// <remarks>The default implementation does nothing.</remarks>
	public virtual void ExitEveryRule([NotNull] ParserRuleContext context) { }
	/// <inheritdoc/>
	/// <remarks>The default implementation does nothing.</remarks>
	public virtual void VisitTerminal([NotNull] ITerminalNode node) { }
	/// <inheritdoc/>
	/// <remarks>The default implementation does nothing.</remarks>
	public virtual void VisitErrorNode([NotNull] IErrorNode node) { }
}
