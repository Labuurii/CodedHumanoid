
ALTER DATABASE pl_war CHARACTER SET utf8;

CREATE TABLE gamers(
	id int PRIMARY KEY AUTO_INCREMENT,
    display_name VARCHAR(255)
);

CREATE TABLE local_logins(
	gamer_id int,
    username VARCHAR(255),
    password_hash VARCHAR(255),
    FOREIGN KEY(gamer_id) REFERENCES gamers(id),
    INDEX(username)
);

CREATE TABLE friends(
	a int,
    b int,
    FOREIGN KEY(a) REFERENCES gamers(id),
    FOREIGN KEY(b) REFERENCES gamers(id),
    INDEX(a), INDEX(b)
);

CREATE TABLE ignored(
	ignorer int,
    ignored int,
    FOREIGN KEY(ignorer) REFERENCES gamers(id),
    FOREIGN KEY(ignored) REFERENCES gamers(id),
    INDEX(ignorer), INDEX(ignored)
);

CREATE TABLE friend_requests(
	from_ int,
    to_ int,
    FOREIGN KEY(from_) REFERENCES gamers(id),
    FOREIGN KEY(to_) REFERENCES gamers(id),
    INDEX(from_), INDEX(to_)
);