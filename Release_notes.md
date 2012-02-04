Cloney 0.9.0.0		2012-02-03
==============================

In this version, Cloney has been rewritten from scratch. It is
now almost entirely SOLIDified, except for the solution cloner
itself. Since it worked perfect in the last release, I think I
will just let it be for now and replace parts of it in a later
release.

This version features some rudimentary routines, including the
following ones:

	<none>: Calling Cloney with no args will start the wizard.
	--help: Displays general help regarding how to use Cloney.
	--clone --source=x --target=y: Clones solution x to dir y.

The wizard has undergone no changes, compared to the previous
version, except that it now calls the natice application with
other parameters. If you want to redesign it, let me know.



Cloney 0.5.0.0		2011-10-09
------------------------------

In this version, focus has been on structural changes and some
minor UI changes. The two applications should work like before,
except some small improvements.

If you download the source code and compare it to any previous
version, you will notice that Cloney has less classes now than
before. The reason to this, is that Cloney now uses the NExtra
library instead of duplicating the classes that exists in it.

The console application has been improved, to be more tolerant
when handling input arguments. It's now possible to use either
/ or \\ when specifying folder paths. earlier, / did cause the
application to crash.

The wizard has some small UI improvements. It is also possible
to enter text by hand into the text boxes, which will probably
make handling the application easier.



Older release notes have been removed
-------------------------------------
