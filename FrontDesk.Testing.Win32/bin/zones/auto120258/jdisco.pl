#Mike Maxim
#inspect

if ($ARGV[0] eq "i") {
    $out = `javac -nowarn *.java 2>&1`;
    $out =~ s/</(/g;
    $out =~ s/>/(/g;
    $out =~ s/&/ /g;
    if (length($out) == 0) {
	system('java JUnitDiscover i');
    } 
    else {
	print "<?xml version=\"1.0\" encoding=\"utf-8\" ?>\n";
	print "<TestSuite xmlns=\"urn:frontdesk-result\">\n";
	print "\t<Error>" . $out . "</Error>\n";
	print "</TestSuite>\n";
    }
}
#compile all the code
else {
    $mike = `javac -nowarn *.java 2>&1`;
    $mike =~ s/</(/g;
    $mike =~ s/>/(/g;
    $mike =~ s/&/ /g;
    if (length($mike) == 0) {
	system('java JUnitDiscover r');
    } 
    else {
	print "<?xml version=\"1.0\" encoding=\"utf-8\" ?>\n";
	print "<Result xmlns=\"urn:frontdesk-result\">\n";
	print "\t<Success>criticallyflawed</Success>\n";
	print "\t<Time>??s</Time>\n";
	print "\t<Count>1</Count>\n";
	print "\t<Msg>" . $mike . "</Msg>\n";
	print "\t<API>JUnit Discovery 1.0</API>\n";
	print "\t<Error>\n";
	print "\t\t<Name>Compile Error</Name>\n";
	print "\t\t<Points>100</Points>\n";
	print "\t\t<Message>" . $mike . "</Message>\n";
	print "\t</Error>\n";
	print "</Result>\n";
    }
}
