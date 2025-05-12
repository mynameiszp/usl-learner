from flask import Flask
from flask_sqlalchemy import SQLAlchemy
from flask_admin import Admin
from flask_admin.contrib.sqla import ModelView
from flask_admin.form import Select2Widget
from wtforms_sqlalchemy.fields import QuerySelectField

# Flask app setup
app = Flask(__name__)
app.config['SECRET_KEY'] = 'supersecretkey'
app.config['SQLALCHEMY_DATABASE_URI'] = 'postgresql://admin:admin@localhost:5432/unitydb'
app.config['SQLALCHEMY_TRACK_MODIFICATIONS'] = False

# Database setup
db = SQLAlchemy(app)

# --- Models ---
class Player(db.Model):
    __tablename__ = 'players'
    id = db.Column(db.Integer, primary_key=True)
    userid = db.Column(db.String)
    name = db.Column(db.String)
    curlevel = db.Column(db.Integer)
    score = db.Column(db.Integer)

class Dictionary(db.Model):
    __tablename__ = 'dictionaries'
    id = db.Column(db.Integer, primary_key=True)
    name = db.Column(db.String)

    def __str__(self):
        return self.name


class Word(db.Model):
    __tablename__ = 'words'
    id = db.Column(db.Integer, primary_key=True)
    name = db.Column(db.String)
    dictionaryid = db.Column(db.Integer, db.ForeignKey('dictionaries.id', ondelete="CASCADE"))
    dictionary = db.relationship('Dictionary', backref='words')

    def __str__(self):
        return self.name


class Level(db.Model):
    __tablename__ = 'levels'
    id = db.Column(db.Integer, primary_key=True)
    level = db.Column(db.Integer)
    points = db.Column(db.Integer)

    def __str__(self):
        return f'Level {self.level}'


class LevelWord(db.Model):
    __tablename__ = 'levelswords'
    id = db.Column(db.Integer, primary_key=True)
    levelid = db.Column(db.Integer, db.ForeignKey('levels.id', ondelete='CASCADE'))
    wordid = db.Column(db.Integer, db.ForeignKey('words.id', ondelete='CASCADE'))
    level = db.relationship('Level', backref='levelwords')
    word = db.relationship('Word', backref='levelwords')


# --- Custom Admin Views ---
class WordAdmin(ModelView):
    column_list = ('id', 'name', 'dictionary')  # Show dictionary in list view
    form_columns = ('name', 'dictionary')       # Show dropdown for dictionary
    form_overrides = {'dictionary': QuerySelectField}
    form_args = {
        'dictionary': {
            'query_factory': lambda: Dictionary.query,
            'get_label': 'name'
        }
    }


class LevelWordAdmin(ModelView):
    form_columns = ['level', 'word']
    form_overrides = {
        'level': QuerySelectField,
        'word': QuerySelectField
    }
    form_args = {
        'level': {
            'query_factory': lambda: Level.query,
            'get_label': 'level'
        },
        'word': {
            'query_factory': lambda: Word.query,
            'get_label': 'name'
        }
    }

# --- Flask-Admin setup ---
admin = Admin(app, name='Unity DB Admin', template_mode='bootstrap3')
admin.add_view(ModelView(Player, db.session))
admin.add_view(ModelView(Dictionary, db.session))
admin.add_view(WordAdmin(Word, db.session))
admin.add_view(ModelView(Level, db.session))
admin.add_view(LevelWordAdmin(LevelWord, db.session))

# --- Run app ---
if __name__ == '__main__':
    app.run(port=5002, debug=True)